using NetflixClone.Domain.Common.Primitives;

namespace NetflixClone.Domain.Entities.Engagement;

public class WatchHistory : BaseEntity
{
    // ── Foreign keys ─────────────────────────────────────────────────
    public Guid ProfileId { get; set; }
    public Guid ContentId { get; set; }

    /// <summary>Null for movies. Set for series episodes.</summary>
    public Guid? EpisodeId { get; set; }

    // ── Progress ──────────────────────────────────────────────────────
    /// <summary>Playback position in seconds when the user last paused or stopped.</summary>
    public int StoppedAtSeconds { get; set; } = 0;

    /// <summary>Full runtime in seconds. Copied from Content/Episode at first watch.</summary>
    public int TotalDurationSeconds { get; set; }

    /// <summary>
    /// True when the user has watched at least 90% of the content.
    /// Excludes this record from Continue Watching; still appears in full Watch History.
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    // ── Timestamps ────────────────────────────────────────────────────
    /// <summary>Last time this record was updated (i.e. last time the user watched).</summary>
    public DateTime WatchedAt { get; set; } = DateTime.UtcNow;

    /// <summary>When IsCompleted was first set to true. Null if not yet completed.</summary>
    public DateTime? CompletedAt { get; set; }

    // ── Navigation ───────────────────────────────────────────────────
    public Identity.Profile Profile { get; set; } = null!;
    public Catalog.Content Content { get; set; } = null!;
}
