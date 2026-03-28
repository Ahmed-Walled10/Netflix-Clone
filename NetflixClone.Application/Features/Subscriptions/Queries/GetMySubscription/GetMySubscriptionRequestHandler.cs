using MediatR;
using NetflixClone.Application.Contracts.Persistence;

namespace NetflixClone.Application.Features.Subscriptions.Queries.GetMySubscription
{
    public class GetMySubscriptionRequestHandler : IRequestHandler<GetMySubscriptionRequest, GetMySubscriptionResponse?>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public GetMySubscriptionRequestHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<GetMySubscriptionResponse?> Handle(
            GetMySubscriptionRequest request,
            CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetActiveByUserIdAsync(request.UserId);
            if (subscription == null)
                return null;

            return new GetMySubscriptionResponse(
                SubscriptionId: subscription.Id,
                PlanName: subscription.Plan?.Name ?? "Unknown Plan",
                Status: subscription.Status.ToString(),
                CurrentPeriodStart: subscription.CurrentPeriodStart,
                CurrentPeriodEnd: subscription.CurrentPeriodEnd,
                CancelAtPeriodEnd: subscription.CancelAtPeriodEnd
            );
        }
    }
}
