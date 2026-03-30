using MediatR;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UploadEpisodeThumbnail;

public class UploadEpisodeThumbnailRequestHandler
    : IRequestHandler<UploadEpisodeThumbnailRequest, UploadEpisodeThumbnailResponse>
{
    private readonly IBaseRepository<Episode> _episodeRepo;
    private readonly ICloudinaryService _cloudinaryService;

    public UploadEpisodeThumbnailRequestHandler(
        IBaseRepository<Episode> episodeRepo,
        ICloudinaryService cloudinaryService)
    {
        _episodeRepo = episodeRepo;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<UploadEpisodeThumbnailResponse> Handle(
        UploadEpisodeThumbnailRequest request,
        CancellationToken cancellationToken)
    {
        var episode = await _episodeRepo.GetByIdAsync(request.EpisodeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Episode {request.EpisodeId} not found.");

        var result = await _cloudinaryService.UploadImageAsync(
            request.FileStream,
            request.FileName,
            $"netflix-clone/episodes/{request.EpisodeId}/thumbnails");

        episode.ThumbnailUrl = result.SecureUrl;

        await _episodeRepo.UpdateAsync(episode);
        await _episodeRepo.SaveChangesAsync(cancellationToken);

        return new UploadEpisodeThumbnailResponse
        {
            ThumbnailUrl = result.SecureUrl
        };
    }
}
