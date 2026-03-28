using MediatR;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UploadContentImages;

/// <summary>
/// Uploads thumbnail and/or hero image for a Content item.
/// At least one stream must be provided.
/// </summary>
public class UploadContentImagesRequest : IRequest<UploadContentImagesResponse>
{
    public Guid ContentId { get; set; }

    public Stream? ThumbnailStream { get; set; }
    public string? ThumbnailFileName { get; set; }

    public Stream? HeroImageStream { get; set; }
    public string? HeroImageFileName { get; set; }
}
