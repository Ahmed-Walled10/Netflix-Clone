using NetflixClone.Domain.Common.Primitives;
using NetflixClone.Domain.Common.Enums;


namespace NetflixClone.Domain.Entities.Catalog;
public class Content : AuditableEntity
{
    public ContentType ContentType { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? OriginalTitle { get; set; }

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Tagline { get; set; }

    public int ReleaseYear { get; set; }

    public int? EndYear { get; set; }

    public int? DurationMinutes { get; set; }

    // ── Age restriction ───────────────────────────────────────────────
    public MaturityRating MaturityRating { get; set; }

    public string OriginalLanguage { get; set; } = "en";

    // ── MVP video URL ─────────────────────────────────────────────────
    /// <summary>
    /// Direct Azure Blob Storage URL for the MP4 file.
    /// Null until an admin uploads and finalizes the video.
    /// MVP simplification — will be replaced by VideoAsset / HLS manifest in v2.
    /// </summary>
    public string? VideoUrl { get; set; }

    /// <summary>Direct URL to trailer video. Optional.</summary>
    public string? TrailerUrl { get; set; }

    // ── Images ───────────────────────────────────────────────────────
    /// <summary>Poster/portrait image shown in browse grids.</summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>Wide landscape image used for hero banners.</summary>
    public string? HeroImageUrl { get; set; }

    // ── Availability ──────────────────────────────────────────────────
    /// <summary>Admin must flip this to true to make content visible to subscribers.</summary>
    public bool IsAvailable { get; set; } = false;

    /// <summary>Marks Netflix Originals / exclusive content.</summary>
    public bool IsOriginal { get; set; } = false;


    // ── Denormalized metrics (updated by domain events / background jobs) ──
    /// <summary>
    /// Running total of all stream-start events. Incremented on every stream start.
    /// Source of truth for trending calculations.
    /// </summary>
    public long ViewCount { get; set; } = 0;

    /// <summary>
    /// Weighted average of all Rating values for this content.
    /// Recalculated by a domain event whenever a rating is added, changed, or removed.
    /// Scale: 0.0 – 5.0
    /// </summary>
    public decimal AverageRating => TotalRatings == 0 ? 0 : (Decimal)Ratings.Sum(r => r.Value) / (TotalRatings * 5);

    /// <summary>Total number of ratings submitted. Used alongside AverageRating.</summary>
    public int TotalRatings { get; set; } = 0;


    // ── Navigation ───────────────────────────────────────────────────
    public ICollection<Season> Seasons { get; set; } = [];
    public ICollection<ContentGenre> ContentGenres { get; set; } = [];
    public ICollection<ContentPerson> ContentPersons { get; set; } = [];
    public ICollection<Engagement.WatchHistory> WatchHistories { get; set; } = [];
    public ICollection<Engagement.Rating> Ratings { get; set; } = [];
}
