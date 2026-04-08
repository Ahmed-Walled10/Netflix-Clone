using MediatR;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.UpdatePlans
{
    public class UpdatePlanCommand : IRequest<bool>
    {
        public Guid PlanId { get; set; }
        public UpdatePlanData Data { get; set; } = new();
    }
}
