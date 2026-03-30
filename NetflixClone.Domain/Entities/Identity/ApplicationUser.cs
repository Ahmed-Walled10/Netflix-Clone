using Microsoft.AspNetCore.Identity;

namespace NetflixClone.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser
{

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

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
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public string? EmailConfirmationOtp { get; set; }
    public DateTime? EmailConfirmationOtpExpiration { get; set; }
    public string? PasswordResetOtp { get; set; }
    public DateTime? PasswordResetOtpExpiration { get; set; }
    public int OtpAttempts { get; set; } = 0;
    public DateTime? LastOtpAttemptAt { get; set; }

    // ── Navigation ──────────────────────────────────────────────────
    public ICollection<Profile> Profiles { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<Subscriptions.Subscription> Subscriptions { get; set; } = [];
}
