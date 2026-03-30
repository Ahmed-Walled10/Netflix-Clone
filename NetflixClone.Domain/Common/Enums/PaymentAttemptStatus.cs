namespace NetflixClone.Domain.Common.Enums
{
    public enum PaymentAttemptStatus
    {
        /// <summary>Charge has been submitted to the payment processor but not yet resolved.</summary>
        Pending = 1,

        /// <summary>Payment was collected successfully.</summary>
        Succeeded = 2,

        /// <summary>Payment was declined (insufficient funds, expired card, fraud block, etc.).</summary>
        Failed = 3,

        /// <summary>Charge was cancelled before settlement (e.g. duplicate prevention).</summary>
        Cancelled = 4
    }
}
