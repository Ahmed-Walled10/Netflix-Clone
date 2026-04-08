using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UpdateContent
{
    public class UpdateContentRequest
    {
        public ContentType? ContentType { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Tagline { get; set; }
        public int? ReleaseYear { get; set; }
        public int? EndYear { get; set; }
        public int? DurationMinutes { get; set; }
        public MaturityRating? MaturityRating { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? VideoUrl { get; set; }
        public string? CloudinaryPublicId { get; set; }
        public string? TrailerUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? HeroImageUrl { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsOriginal { get; set; }
    }
}
