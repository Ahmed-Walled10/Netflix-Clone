using NetflixClone.Domain.Common.Primitives;

namespace NetflixClone.Domain.Entities.Identity;

public class RefreshToken : BaseEntity
{
    // ── Foreign key ──────────────────────────────────────────────────
    public string UserId { get; set; } = string.Empty;

    // ── Token data ───────────────────────────────────────────────────
    /// <summary>
    /// SHA-256 hash of the random token sent to the client.
    /// Never store or log the plain token server-side.
    /// </summary>
    public string TokenHash { get; set; } = string.Empty;

    /// <summary>User-Agent string of the device that created this token.</summary>
    public string? DeviceInfo { get; set; }

    /// <summary>IP address at the time of token creation. Supports IPv6 (max 45 chars).</summary>
    public string? IpAddress { get; set; }

    // ── Lifecycle ────────────────────────────────────────────────────
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Set when this token is consumed (rotated) or explicitly revoked (logout).</summary>
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Hash of the token that replaced this one on rotation.
    /// Allows tracing the rotation chain for security audits.
    /// </summary>
    public string? ReplacedByTokenHash { get; set; }

    // ── Computed helpers ─────────────────────────────────────────────
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsActive => !IsExpired && !IsRevoked;

    // ── Navigation ───────────────────────────────────────────────────
    public ApplicationUser User { get; set; } = null!;
}
