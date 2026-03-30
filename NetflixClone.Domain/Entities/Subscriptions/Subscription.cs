using NetflixClone.Domain.Common.Primitives;
using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Domain.Entities.Subscriptions;

public class Subscription : AuditableEntity
{
    // ── Foreign keys ─────────────────────────────────────────────────
    public string UserId { get; set; } = string.Empty;
    public Guid PlanId { get; set; }

    // ── Status ───────────────────────────────────────────────────────
    public SubscriptionStatus Status { get; set; } 

    // ── Billing period ───────────────────────────────────────────────
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }

    /// <summary>
    /// When the user requested cancellation.
    /// Access continues until CurrentPeriodEnd. Status = Canceled after that.
    /// </summary>
    public DateTime? CanceledAt { get; set; }

    ///subscription will automatically renew at the end of the billing period.
    public bool AutoRenew { get; set; } = true;

    // ── Stripe ───────────────────────────────────────────────────────
    /// <summary>Stripe Subscription ID (sub_xxxx). Unique per active subscription.</summary>
    public string? StripeSubscriptionId { get; set; }

    // ── Navigation ───────────────────────────────────────────────────
    public Identity.ApplicationUser User { get; set; } = null!;
    public Plan Plan { get; set; } = null!;
    public ICollection<Invoice> Invoices { get; set; } = [];
}
