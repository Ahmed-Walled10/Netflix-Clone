using MediatR;

namespace NetflixClone.Application.Features.Subscriptions.Queries.GetMySubscription
{
    public class GetMySubscriptionRequest : IRequest<GetMySubscriptionResponse?>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
