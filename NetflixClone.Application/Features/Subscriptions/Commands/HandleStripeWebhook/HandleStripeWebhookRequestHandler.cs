using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Identity;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Application.Features.Subscriptions.Commands.HandleStripeWebhook
{
    public class HandleStripeWebhookRequestHandler : IRequestHandler<HandleStripeWebhookRequest, Unit>
    {
        private readonly IStripeService _stripeService;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IBaseRepository<Subscription> _baseSubRepository;
        private readonly IBaseRepository<Plan> _planRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HandleStripeWebhookRequestHandler> _logger;

        public HandleStripeWebhookRequestHandler(
            IStripeService stripeService,
            ISubscriptionRepository subscriptionRepository,
            IBaseRepository<Subscription> baseSubRepository,
            IBaseRepository<Plan> planRepository,
            IEmailService emailService,
            UserManager<ApplicationUser> userManager,
            ILogger<HandleStripeWebhookRequestHandler> logger)
        {
            _stripeService = stripeService;
            _subscriptionRepository = subscriptionRepository;
            _baseSubRepository = baseSubRepository;
            _planRepository = planRepository;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Unit> Handle(HandleStripeWebhookRequest request, CancellationToken cancellationToken)
        {
            // 1. Verify signature and parse the event
            StripeWebhookEvent webhookEvent;
            try
            {
                webhookEvent = await _stripeService.ConstructWebhookEventAsync(
                    request.JsonPayload, request.StripeSignatureHeader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid Stripe webhook signature.");
                throw;
            }

            // 2. Handle the event
            switch (webhookEvent.EventType)
            {
                case "checkout.session.completed":
                    await HandleCheckoutCompletedAsync(webhookEvent, cancellationToken);
                    break;

                default:
                    _logger.LogInformation("Unhandled Stripe event type: {Type}", webhookEvent.EventType);
                    break;
            }

            return Unit.Value;
        }

        private async Task HandleCheckoutCompletedAsync(
            StripeWebhookEvent webhookEvent,
            CancellationToken cancellationToken)
        {
            // 1. Find the user by Stripe Customer ID
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.StripeCustomerId == webhookEvent.StripeCustomerId, cancellationToken);

            if (user == null)
            {
                _logger.LogError("User not found for Stripe Customer {CustomerId}", webhookEvent.StripeCustomerId);
                return;
            }

            // 2. Get the Plan from metadata
            if (!webhookEvent.Metadata.TryGetValue("planId", out var planIdStr) ||
                !Guid.TryParse(planIdStr, out var planId))
            {
                _logger.LogError("planId missing from webhook metadata.");
                return;
            }

            var plan = await _planRepository.GetByIdAsync(planId, cancellationToken);
            if (plan == null)
            {
                _logger.LogError("Plan {PlanId} not found.", planId);
                return;
            }

            // 3. Prevent duplicate subscriptions
            var existingSub = await _subscriptionRepository.GetActiveByUserIdAsync(user.Id);
            if (existingSub != null)
            {
                _logger.LogWarning("User {UserId} already has an active subscription. Skipping.", user.Id);
                return;
            }

            // 4. Create local Subscription record
            var now = DateTime.UtcNow;
            var periodStart = webhookEvent.PeriodStart ?? now;
            var periodEnd = webhookEvent.PeriodEnd ?? now.AddMonths(1);

            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                PlanId = plan.Id,
                Status = SubscriptionStatus.Active,
                CurrentPeriodStart = periodStart,
                CurrentPeriodEnd = periodEnd,
                AutoRenew = true,
                CancelAtPeriodEnd = false,
                StripeSubscriptionId = webhookEvent.StripeSubscriptionId,
                StripeCustomerId = webhookEvent.StripeCustomerId
            };

            await _baseSubRepository.AddAsync(subscription, cancellationToken);
            await _baseSubRepository.SaveChangesAsync(cancellationToken);

            // 5. Create Invoice record
            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                SubscriptionId = subscription.Id,
                Amount = webhookEvent.AmountPaid ?? plan.Price,
                Currency = webhookEvent.Currency ?? "usd",
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                PaidAt = now,
                StripeInvoiceId = webhookEvent.StripeInvoiceId ?? string.Empty,
                StripePdfUrl = webhookEvent.InvoicePdfUrl
            };

            await _subscriptionRepository.AddInvoiceAsync(invoice);

            // 6. Change role: NotSubscriber → Subscriber
            if (await _userManager.IsInRoleAsync(user, "NotSubscriber"))
                await _userManager.RemoveFromRoleAsync(user, "NotSubscriber");

            if (!await _userManager.IsInRoleAsync(user, "Subscriber"))
                await _userManager.AddToRoleAsync(user, "Subscriber");

            // 7. Send invoice email
            await _emailService.SendInvoiceEmailAsync(
                user.Email!,
                user.FirstName,
                plan.Name,
                invoice.Amount,
                "Card",
                "****",
                now,
                periodEnd);

            _logger.LogInformation(
                "Subscription created for user {UserId}, plan {PlanName}, invoice {InvoiceId}",
                user.Id, plan.Name, invoice.Id);
        }
    }
}
