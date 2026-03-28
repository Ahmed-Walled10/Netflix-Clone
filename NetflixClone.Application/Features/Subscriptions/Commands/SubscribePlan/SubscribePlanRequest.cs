using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Subscriptions.Commands.SubscribePlan
{
    public class SubscribePlanRequest : IRequest<SubscribePlanResponse>
    {
        [Required]
        public Guid PlanId { get; init; }

        // Set by the controller from JWT claims — not sent by the client
        public string UserId { get; set; } = string.Empty;
    }
}
