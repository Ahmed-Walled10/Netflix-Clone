using NetflixClone.Domain.Common.Primitives;
using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Domain.Entities.Subscription;

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

    /// <summary>
    /// True = user requested cancel but has access until CurrentPeriodEnd.
    /// Stripe will not renew and will call customer.subscription.deleted at period end.
    /// </summary>
    public bool CancelAtPeriodEnd { get; set; } = false;

    // ── Stripe ───────────────────────────────────────────────────────
    /// <summary>Stripe Subscription ID (sub_xxxx). Unique per active subscription.</summary>
    public string? StripeSubscriptionId { get; set; }

    // ── Navigation ───────────────────────────────────────────────────
    public Identity.ApplicationUser User { get; set; } = null!;
    public Plan Plan { get; set; } = null!;
    public ICollection<Invoice> Invoices { get; set; } = [];
}
