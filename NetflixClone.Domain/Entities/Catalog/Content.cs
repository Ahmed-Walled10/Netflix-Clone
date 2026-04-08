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

    // ── Video ─────────────────────────────────────────────────────────
    /// <summary>
    /// Direct URL for the MP4 file.
    /// Null until an admin uploads and finalizes the video.
    /// </summary>
    public string? VideoUrl { get; set; }

    /// <summary>Cloudinary public ID of the uploaded video. Used to build quality-specific streaming URLs.</summary>
    public string? CloudinaryPublicId { get; set; }

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
    /// Average of all Rating values for this content. Scale: 0.0 – 5.0.
    /// Updated by Application layer handlers when a rating is added, changed, or removed.
    /// </summary>
    public decimal AverageRating { get; set; } = 0;

    /// <summary>Total number of ratings submitted. Used alongside AverageRating.</summary>
    public int TotalRatings { get; set; } = 0;


    // ── Navigation ───────────────────────────────────────────────────
    public ICollection<Season> Seasons { get; set; } = [];
    public ICollection<ContentGenre> ContentGenres { get; set; } = [];
    public ICollection<ContentPerson> ContentPersons { get; set; } = [];
    public ICollection<Engagement.WatchHistory> WatchHistories { get; set; } = [];
    public ICollection<Engagement.Rating> Ratings { get; set; } = [];

    // ── Behavior ─────────────────────────────────────────────────────
    public void Update(UpdateContentData data)
    {
        if (data.ContentType.HasValue) ContentType = data.ContentType.Value;
        if (data.Title != null) Title = data.Title;
        if (data.OriginalTitle != null) OriginalTitle = data.OriginalTitle;
        if (data.Slug != null) Slug = data.Slug;
        if (data.Description != null) Description = data.Description;
        if (data.Tagline != null) Tagline = data.Tagline;
        if (data.ReleaseYear.HasValue) ReleaseYear = data.ReleaseYear.Value;
        if (data.EndYear.HasValue) EndYear = data.EndYear.Value;
        if (data.DurationMinutes.HasValue) DurationMinutes = data.DurationMinutes.Value;
        if (data.MaturityRating.HasValue) MaturityRating = data.MaturityRating.Value;
        if (data.OriginalLanguage != null) OriginalLanguage = data.OriginalLanguage;
        if (data.VideoUrl != null) VideoUrl = data.VideoUrl;
        if (data.CloudinaryPublicId != null) CloudinaryPublicId = data.CloudinaryPublicId;
        if (data.TrailerUrl != null) TrailerUrl = data.TrailerUrl;
        if (data.ThumbnailUrl != null) ThumbnailUrl = data.ThumbnailUrl;
        if (data.HeroImageUrl != null) HeroImageUrl = data.HeroImageUrl;
        if (data.IsAvailable.HasValue) IsAvailable = data.IsAvailable.Value;
        if (data.IsOriginal.HasValue) IsOriginal = data.IsOriginal.Value;
    }
}

public class UpdateContentData
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
