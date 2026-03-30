using NetflixClone.Domain.Common.Primitives;

namespace NetflixClone.Domain.Entities.Catalog;

public class Episode : AuditableEntity
{
    // ── Foreign key ──────────────────────────────────────────────────
    public Guid SeasonId { get; set; }

    // ── Data ─────────────────────────────────────────────────────────
    /// <summary>Ordinal position within the season: 1, 2, 3 … Unique per SeasonId.</summary>
    public int EpisodeNumber { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public int? ReleaseYear { get; set; }
    public string? ThumbnailUrl { get; set; }

    /// <summary>Direct URL for this episode's MP4 video. Null until uploaded.</summary>
    public string? VideoUrl { get; set; }

    /// <summary>Cloudinary public ID of the uploaded video. Used to build quality-specific streaming URLs.</summary>
    public string? CloudinaryPublicId { get; set; }

    /// <summary>False until the admin marks the episode ready for streaming.</summary>
    public bool IsAvailable { get; set; } = false;

    /// <summary>Original broadcast / release date. Null for streaming-only originals.</summary>
    public DateOnly? AirDate { get; set; }

    // ── Navigation ───────────────────────────────────────────────────
    public Season Season { get; set; } = null!;
}
