namespace NetflixClone.Application.Features.Subscriptions.Queries.GetMySubscription
{
    public record GetMySubscriptionResponse(
        Guid SubscriptionId,
        string PlanName,
        string Status,
        DateTime CurrentPeriodStart,
        DateTime CurrentPeriodEnd,
        bool CancelAtPeriodEnd
    );
}
