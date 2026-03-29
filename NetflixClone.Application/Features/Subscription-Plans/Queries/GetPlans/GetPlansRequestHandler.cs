using MediatR;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Application.Features.Subscription_Plans.Queries.GetPlans
{
    public class GetPlansRequestHandler : IRequestHandler<GetPlansRequest, GetPlansResponce>
    {
        private readonly IBaseRepository<Plan> _Plansrepository;

        public GetPlansRequestHandler(
            IBaseRepository<Plan> Plansrepository)

        {
            _Plansrepository = Plansrepository;
        }

        public async Task<GetPlansResponce> Handle(GetPlansRequest request, CancellationToken cancellationToken)
        {
            var plans = await _Plansrepository.GetAllAsync(cancellationToken);
            if (plans is null || !plans.Any())
                throw new Exception("Plans were not found. Please try again later.");

            var planDtos = plans.Select(plan => new SubscriptionPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                Description = plan.DisplayName, // Assuming DisplayName is used as Description
                MaxProfiles = plan.MaxProfiles,
                VideoQuality = plan.MaxVideoQuality.ToString()
            }).ToList();

            return new GetPlansResponce { Plans = planDtos, Success = true, Message = null };
        }
    }
}
