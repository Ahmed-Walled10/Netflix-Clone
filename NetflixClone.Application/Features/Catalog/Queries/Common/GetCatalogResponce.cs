using NetflixClone.Domain.Common.Enums;
using System;
using System.Collections.Generic;

namespace NetflixClone.Application.Features.Catalog.Queries.Common
{
    public class GetCatalogResponce
    {
        public Guid Id { get; set; }
        public ContentType ContentType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public MaturityRating MaturityRating { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? HeroImageUrl { get; set; }
        public decimal AverageRating { get; set; }
        public long ViewCount { get; set; }
    }
}
