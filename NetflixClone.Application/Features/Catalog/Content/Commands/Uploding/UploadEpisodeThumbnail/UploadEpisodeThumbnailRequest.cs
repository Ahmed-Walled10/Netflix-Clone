using MediatR;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UploadEpisodeThumbnail;

    /// <summary>
    /// Uploads a thumbnail image for a specific Episode.
    /// </summary>
    public class UploadEpisodeThumbnailRequest : IRequest<UploadEpisodeThumbnailResponse>
    {
        public Guid EpisodeId { get; set; }
        public Stream FileStream { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
}
