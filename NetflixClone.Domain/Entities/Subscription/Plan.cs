using NetflixClone.Domain.Common.Primitives;
using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Domain.Entities.Subscription;

public class Plan : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    /// <summary>Human-readable display name, e.g. "Premium Yearly".</summary>
    public string DisplayName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public BillingPeriod BillingPeriod { get; set; }

    /// <summary>Max number of profiles the account holder can create under this plan.</summary>
    public int MaxProfiles { get; set; }

    /// <summary>
    /// Stripe Price ID (price_xxxx) for this plan.
    /// Used to create Checkout Sessions.
    /// Null only during local development before Stripe is configured.
    /// </summary>
    public string? StripePriceId { get; set; }

    /// <summary>False = plan is retired and not shown on the pricing page.</summary>
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ── Navigation ───────────────────────────────────────────────────
    public ICollection<Subscription> Subscriptions { get; set; } = [];
}
