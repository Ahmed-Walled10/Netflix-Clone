using MediatR;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Catalog;
using ContentEntity = NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UploadContentVideo;

public class UploadContentVideoRequestHandler
    : IRequestHandler<UploadContentVideoRequest, UploadContentVideoResponse>
{
    private readonly IBaseRepository<ContentEntity> _contentRepo;
    private readonly IBaseRepository<Episode> _episodeRepo;
    private readonly ICloudinaryService _cloudinaryService;

    public UploadContentVideoRequestHandler(
        IBaseRepository<ContentEntity> contentRepo,
        IBaseRepository<Episode> episodeRepo,
        ICloudinaryService cloudinaryService)
    {
        _contentRepo = contentRepo;
        _episodeRepo = episodeRepo;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<UploadContentVideoResponse> Handle(
        UploadContentVideoRequest request,
        CancellationToken cancellationToken)
    {
        // ── Upload to specific episode ───────────────────────────────────────
        if (request.EpisodeId.HasValue)
        {
            var episode = await _episodeRepo.GetByIdAsync(request.EpisodeId.Value, cancellationToken)
                ?? throw new KeyNotFoundException($"Episode {request.EpisodeId.Value} not found.");

            var result = await _cloudinaryService.UploadVideoAsync(
                request.FileStream,
                request.FileName,
                $"netflix-clone/episodes/{request.ContentId}");

            episode.VideoUrl = result.SecureUrl;
            episode.CloudinaryPublicId = result.PublicId;

            await _episodeRepo.UpdateAsync(episode);
            await _episodeRepo.SaveChangesAsync(cancellationToken);

            return new UploadContentVideoResponse
            {
                VideoUrl = result.SecureUrl,
                PublicId = result.PublicId
            };
        }

        // ── Upload to content (movie / documentary) ──────────────────────────
        var content = await _contentRepo.GetByIdAsync(request.ContentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Content {request.ContentId} not found.");

        var uploadResult = await _cloudinaryService.UploadVideoAsync(
            request.FileStream,
            request.FileName,
            $"netflix-clone/content/{request.ContentId}");

        content.VideoUrl = uploadResult.SecureUrl;
        content.CloudinaryPublicId = uploadResult.PublicId;

        await _contentRepo.UpdateAsync(content);
        await _contentRepo.SaveChangesAsync(cancellationToken);

        return new UploadContentVideoResponse
        {
            VideoUrl = uploadResult.SecureUrl,
            PublicId = uploadResult.PublicId
        };
    }
}
