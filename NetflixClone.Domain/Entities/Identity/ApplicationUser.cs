using Microsoft.AspNetCore.Identity;

namespace NetflixClone.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    // ── Stripe ──────────────────────────────────────────────────────
    /// <summary>
    /// The Stripe Customer ID for this account (cus_xxxx).
    /// Created on first checkout. Null until then.
    /// </summary>
    public string? StripeCustomerId { get; set; }

    // ── Soft delete ──────────────────────────────────────────────────
    /// <summary>
    /// False means the account has been deactivated.
    /// Data is retained for billing history. User cannot log in.
    /// </summary>
    public bool IsActive { get; set; } = true;

    public bool IsSuspended { get; set; } = false;
    public string? SuspensionReason { get; set; }

    // ── Audit ────────────────────────────────────────────────────────
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // ── Navigation ──────────────────────────────────────────────────
    public ICollection<Profile> Profiles { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<Subscription.Subscription> Subscriptions { get; set; } = [];
    public ICollection<Subscription.PaymentMethod> PaymentMethods { get; set; } = [];
}
