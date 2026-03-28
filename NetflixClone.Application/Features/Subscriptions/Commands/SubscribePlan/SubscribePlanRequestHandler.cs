using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Identity;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Application.Features.Subscriptions.Commands.SubscribePlan
{
    public class SubscribePlanRequestHandler : IRequestHandler<SubscribePlanRequest, SubscribePlanResponse>
    {
        private readonly IBaseRepository<Plan> _planRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IStripeService _stripeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscribePlanRequestHandler(
            IBaseRepository<Plan> planRepository,
            ISubscriptionRepository subscriptionRepository,
            IStripeService stripeService,
            UserManager<ApplicationUser> userManager)
        {
            _planRepository = planRepository;
            _subscriptionRepository = subscriptionRepository;
            _stripeService = stripeService;
            _userManager = userManager;
        }

        public async Task<SubscribePlanResponse> Handle(
            SubscribePlanRequest request,
            CancellationToken cancellationToken)
        {
            // 1. Get the user
            var user = await _userManager.FindByIdAsync(request.UserId)
                ?? throw new UnauthorizedAccessException("User not found.");

            // 2. Get the plan
            var plan = await _planRepository.GetByIdAsync(request.PlanId, cancellationToken)
                ?? throw new KeyNotFoundException($"Plan with Id {request.PlanId} was not found.");

            if (!plan.IsActive)
                throw new InvalidOperationException("This plan is no longer available.");

            if (string.IsNullOrEmpty(plan.StripePriceId))
                throw new InvalidOperationException($"Plan '{plan.Name}' doesn't have a configured StripePriceId.");

            // 3. Check for existing active subscription
            var activeSub = await _subscriptionRepository.GetActiveByUserIdAsync(user.Id);
            if (activeSub != null)
                throw new InvalidOperationException("You already have an active subscription. You can only upgrade to a higher plan.");

            // 4. Get or create Stripe Customer
            var customerId = await _stripeService.CreateOrGetCustomerAsync(user.Id, user.Email!);
            if (string.IsNullOrEmpty(user.StripeCustomerId) || user.StripeCustomerId != customerId)
            {
                user.StripeCustomerId = customerId;
                await _userManager.UpdateAsync(user);
            }

            // 5. Create Stripe Checkout Session and return the URL
            var session = await _stripeService.CreateCheckoutSessionAsync(
                customerId, plan.StripePriceId, plan.Id);

            return new SubscribePlanResponse(session.Url);
        }
    }
}
