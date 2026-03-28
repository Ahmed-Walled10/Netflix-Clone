using NetflixClone.Domain.Common.Primitives;
using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Domain.Entities.Subscriptions;

public class Invoice : BaseEntity
{
    // ── Foreign key ──────────────────────────────────────────────────
    public Guid SubscriptionId { get; set; }

    // ── Amount ───────────────────────────────────────────────────────
    public decimal Amount { get; set; }

    /// <summary>ISO 4217 currency code, e.g. "usd". Lowercased to match Stripe.</summary>
    public string Currency { get; set; } = "usd";

    // ── Billing period this payment covers ───────────────────────────
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }

    /// <summary>UTC timestamp when Stripe confirmed the charge succeeded.</summary>
    public DateTime PaidAt { get; set; }

    // ── Stripe reference ─────────────────────────────────────────────
    /// <summary>Stripe Invoice ID (in_xxxx). Unique — used to prevent duplicate processing.</summary>
    public string StripeInvoiceId { get; set; } = string.Empty;

    /// <summary>
    /// URL to the Stripe-hosted PDF invoice.
    /// Included in the invoice email so the user can download it.
    /// </summary>
    public string? StripePdfUrl { get; set; }

    // ── Email tracking ────────────────────────────────────────────────
    public DateTime? EmailSentAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ── Navigation ───────────────────────────────────────────────────
    public Subscription Subscription { get; set; } = null!;
}
