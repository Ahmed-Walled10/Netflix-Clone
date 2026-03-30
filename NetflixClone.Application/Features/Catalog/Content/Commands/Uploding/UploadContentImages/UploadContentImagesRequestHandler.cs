using MediatR;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using ContentEntity = NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UploadContentImages;

public class UploadContentImagesRequestHandler
    : IRequestHandler<UploadContentImagesRequest, UploadContentImagesResponse>
{
    private readonly IBaseRepository<ContentEntity> _contentRepo;
    private readonly ICloudinaryService _cloudinaryService;

    public UploadContentImagesRequestHandler(
        IBaseRepository<ContentEntity> contentRepo,
        ICloudinaryService cloudinaryService)
    {
        _contentRepo = contentRepo;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<UploadContentImagesResponse> Handle(
        UploadContentImagesRequest request,
        CancellationToken cancellationToken)
    {
        var content = await _contentRepo.GetByIdAsync(request.ContentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Content {request.ContentId} not found.");

        var response = new UploadContentImagesResponse();

        // ── Thumbnail ────────────────────────────────────────────────────────
        if (request.ThumbnailStream is not null && !string.IsNullOrWhiteSpace(request.ThumbnailFileName))
        {
            var result = await _cloudinaryService.UploadImageAsync(
                request.ThumbnailStream,
                request.ThumbnailFileName,
                $"netflix-clone/content/{request.ContentId}/thumbnails");

            content.ThumbnailUrl = result.SecureUrl;
            response.ThumbnailUrl = result.SecureUrl;
        }

        // ── Hero Image ───────────────────────────────────────────────────────
        if (request.HeroImageStream is not null && !string.IsNullOrWhiteSpace(request.HeroImageFileName))
        {
            var result = await _cloudinaryService.UploadImageAsync(
                request.HeroImageStream,
                request.HeroImageFileName,
                $"netflix-clone/content/{request.ContentId}/heroes");

            content.HeroImageUrl = result.SecureUrl;
            response.HeroImageUrl = result.SecureUrl;
        }

        await _contentRepo.UpdateAsync(content);
        await _contentRepo.SaveChangesAsync(cancellationToken);

        return response;
    }
}
