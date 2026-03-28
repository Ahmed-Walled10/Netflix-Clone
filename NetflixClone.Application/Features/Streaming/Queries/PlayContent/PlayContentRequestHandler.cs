using MediatR;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Catalog;
using NetflixClone.Domain.Entities.Engagement;
using ContentEntity = NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Streaming.Queries.PlayContent;

public class PlayContentRequestHandler
    : IRequestHandler<PlayContentRequest, PlayContentResponse>
{
    private readonly IBaseRepository<ContentEntity> _contentRepo;
    private readonly IBaseRepository<Episode> _episodeRepo;
    private readonly IBaseRepository<WatchHistory> _watchHistoryRepo;
    private readonly IWatchHistoryRepository _watchHistoryQueryRepo;
    private readonly ISubscriptionRepository _subscriptionRepo;
    private readonly ICloudinaryService _cloudinaryService;

    public PlayContentRequestHandler(
        IBaseRepository<ContentEntity> contentRepo,
        IBaseRepository<Episode> episodeRepo,
        IBaseRepository<WatchHistory> watchHistoryRepo,
        IWatchHistoryRepository watchHistoryQueryRepo,
        ISubscriptionRepository subscriptionRepo,
        ICloudinaryService cloudinaryService)
    {
        _contentRepo = contentRepo;
        _episodeRepo = episodeRepo;
        _watchHistoryRepo = watchHistoryRepo;
        _watchHistoryQueryRepo = watchHistoryQueryRepo;
        _subscriptionRepo = subscriptionRepo;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<PlayContentResponse> Handle(
        PlayContentRequest request,
        CancellationToken cancellationToken)
    {
        // user's subscription plan quality
        var subscription = await _subscriptionRepo.GetActiveByUserIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException("No active subscription found. Please subscribe to a plan.");

        var maxQuality = subscription.Plan.MaxVideoQuality;

        // Find the content / episode and its Cloudinary public ID
        string? publicId;
        ContentEntity? content = null;

        if (request.EpisodeId.HasValue)
        {
            var episode = await _episodeRepo.GetByIdAsync(request.EpisodeId.Value, cancellationToken)
                ?? throw new KeyNotFoundException($"Episode {request.EpisodeId.Value} not found.");

            if (!episode.IsAvailable)
                throw new InvalidOperationException("This episode is not available for streaming yet.");

            publicId = episode.CloudinaryPublicId
                ?? throw new InvalidOperationException("This episode does not have a video uploaded yet.");
        }
        else
        {
            content = await _contentRepo.GetByIdAsync(request.ContentId, cancellationToken)
                ?? throw new KeyNotFoundException($"Content {request.ContentId} not found.");

            if (!content.IsAvailable)
                throw new InvalidOperationException("This content is not available for streaming yet.");

            publicId = content.CloudinaryPublicId
                ?? throw new InvalidOperationException("This content does not have a video uploaded yet.");
        }

        // Build the quality-constrained streaming URL
        var streamingUrl = _cloudinaryService.GetVideoUrl(publicId, maxQuality);

        // Add / update watch history for this profile
        var existingHistory = await _watchHistoryQueryRepo.GetByProfileAndContentAsync(
            request.ProfileId,
            request.ContentId,
            request.EpisodeId,
            cancellationToken);

        if (existingHistory is not null)
        {
            // User is re-watching — update the timestamp, reset completion
            existingHistory.WatchedAt = DateTime.UtcNow;
            existingHistory.IsCompleted = false;
            existingHistory.CompletedAt = null;
            existingHistory.StoppedAtSeconds = 0;
            await _watchHistoryRepo.UpdateAsync(existingHistory);
        }
        else
        {
            var watchHistory = new WatchHistory
            {
                Id = Guid.NewGuid(),
                ProfileId = request.ProfileId,
                ContentId = request.ContentId,
                EpisodeId = request.EpisodeId,
                StoppedAtSeconds = 0,
                IsCompleted = false,
                WatchedAt = DateTime.UtcNow
            };

            await _watchHistoryRepo.AddAsync(watchHistory, cancellationToken);
        }

        await _watchHistoryRepo.SaveChangesAsync(cancellationToken);

        // Increment view count
        if (content is not null)
        {
            content.ViewCount++;
            await _contentRepo.UpdateAsync(content);
            await _contentRepo.SaveChangesAsync(cancellationToken);
        }

        return new PlayContentResponse
        {
            StreamingUrl = streamingUrl,
            Quality = maxQuality.ToString()
        };
    }
}
