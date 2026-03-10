
namespace NetflixClone.Domain.Common.Enums
{
    public enum SubscriptionStatus
    {
        /// <summary>Subscription is active and in good standing.</summary>
        Active = 1,

        /// <summary>User requested cancellation. Access continues until CurrentPeriodEnd.</summary>
        Canceled = 2,

        /// <summary>Payment failed; Stripe is retrying. User still has access during retry window.</summary>
        PastDue = 3,

        /// <summary>Subscription period ended and was not renewed. No access.</summary>
        Expired = 4
    }
}
