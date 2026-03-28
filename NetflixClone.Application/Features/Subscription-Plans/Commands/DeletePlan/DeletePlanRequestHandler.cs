using MediatR;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.DeletePlan
{
    public class DeletePlanRequestHandler : IRequestHandler<DeletePlanRequest, Unit>
    {
        private readonly IBaseRepository<Plan> _planRepo; 

        public DeletePlanRequestHandler(
            IBaseRepository<Plan> planRepo)
        {
            _planRepo = planRepo;
        }

        public async Task<Unit> Handle(DeletePlanRequest request, CancellationToken cancellationToken)
        {
            var plan = await _planRepo.GetByIdAsync(request.Id, cancellationToken);
            if (plan == null)
                throw new Exception($"Plan with ID {request.Id} not found.");

            await _planRepo.DeleteAsync(plan, cancellationToken);
            return Unit.Value;
        }
    }
}
