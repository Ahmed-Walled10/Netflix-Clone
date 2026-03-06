using NetflixClone.Domain.Common.Primitives;

namespace NetflixClone.Domain.Entities.Subscriptions;

public class PaymentMethod : BaseEntity
{
    // ── Foreign key ──────────────────────────────────────────────────
    public string UserId { get; set; } = string.Empty;

    // ── Stripe reference ─────────────────────────────────────────────
    /// <summary>Stripe PaymentMethod ID (pm_xxxx). Unique per card.</summary>
    public string StripePaymentMethodId { get; set; } = string.Empty;

    // ── Display info (read from Stripe, stored locally for fast display) ──
    /// <summary>Card brand as returned by Stripe: "visa", "mastercard", "amex", etc.</summary>
    public string Brand { get; set; } = string.Empty;

    /// <summary>Last 4 digits of the card number. Safe to display.</summary>
    public string Last4 { get; set; } = string.Empty;

    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }

    // ── Default flag ─────────────────────────────────────────────────
    /// <summary>Only one PaymentMethod per UserId can be IsDefault = true.</summary>
    public bool IsDefault { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ── Navigation ───────────────────────────────────────────────────
    public Identity.ApplicationUser User { get; set; } = null!;
}
