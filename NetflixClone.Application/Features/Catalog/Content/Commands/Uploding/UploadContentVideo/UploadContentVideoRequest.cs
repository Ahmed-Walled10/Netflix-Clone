using MediatR;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UploadContentVideo;

/// <summary>
/// Uploads a video file for a Content (movie) or a specific Episode (series).
/// If EpisodeId is provided, the video is linked to that episode.
/// If EpisodeId is null, the video is linked to the Content itself (movie/documentary).
/// </summary>
public class UploadContentVideoRequest : IRequest<UploadContentVideoResponse>
{
    public Guid ContentId { get; set; }
    public Guid? EpisodeId { get; set; }
    public Stream FileStream { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
}
