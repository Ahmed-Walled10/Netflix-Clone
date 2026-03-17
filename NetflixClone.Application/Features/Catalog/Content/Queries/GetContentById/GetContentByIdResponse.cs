using NetflixClone.Domain.Common.Enums;
using System;
using System.Collections.Generic;

namespace NetflixClone.Application.Features.Catalog.Content.Queries.GetContentById
{
    public class GetContentByIdResponse
    {
        public Guid Id { get; set; }
        public ContentType ContentType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? OriginalTitle { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Tagline { get; set; }
        public int ReleaseYear { get; set; }
        public int? EndYear { get; set; }
        public int? DurationMinutes { get; set; }
        public MaturityRating MaturityRating { get; set; }
        public string OriginalLanguage { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? HeroImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsOriginal { get; set; }
        public long ViewCount { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalRatings { get; set; }

        public List<ContentCastDto> Cast { get; set; } = new();
    }
}
