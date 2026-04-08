using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.UpdatePlans
{
    public class UpdatePlanRequestHandler : IRequestHandler<UpdatePlanCommand, bool>
    {
        private readonly IBaseRepository<Plan> _planBaseRepository;

        public UpdatePlanRequestHandler(IBaseRepository<Plan> planBaseRepository)
        {
            _planBaseRepository = planBaseRepository;
        }

        public async Task<bool> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _planBaseRepository.GetByIdAsync(request.PlanId);

            if (plan == null)
                throw new KeyNotFoundException($"Plan with Id {request.PlanId} not found.");

            plan.Update(request.Data);

            await _planBaseRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
