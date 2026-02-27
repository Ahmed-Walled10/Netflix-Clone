using NetflixClone.Domain.Common.Primitives;
using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Domain.Entities.Media;

public class StreamingSession : BaseEntity
{
    // ── Foreign keys ─────────────────────────────────────────────────
    public Guid ProfileId { get; set; }
    public Guid ContentId { get; set; }

    /// <summary>Episode being streamed. Null for movies / documentaries.</summary>
    public Guid? EpisodeId { get; set; }

    // ── Device info ───────────────────────────────────────────────────
    public string? DeviceType { get; set; }

    /// <summary>Unique device identifier sent by the client app.</summary>
    public string? DeviceId { get; set; }

    public string? IpAddress { get; set; }

      // Will be added in the future
   /* // ── Quality ───────────────────────────────────────────────────────
    /// <summary>The resolution this session is streaming at. Capped by Plan.MaxVideoQuality.</summary>
    public VideoQuality Quality { get; set; } = VideoQuality.FullHD_1080p;*/

    // ── Lifecycle ─────────────────────────────────────────────────────
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Updated every 30 seconds by the client heartbeat endpoint.
    /// If this is older than 2 minutes, the session is considered dead / stale.
    /// </summary>
    public DateTime LastHeartbeatAt { get; set; } = DateTime.UtcNow;

    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// True = session is alive (client actively streaming or heartbeat is recent).
    /// False = session ended (client called end-session) or marked stale by background job.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // ── Navigation ───────────────────────────────────────────────────
    public Identity.Profile Profile { get; set; } = null!;
}
