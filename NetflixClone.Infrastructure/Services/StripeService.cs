using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Infrastructure.Options;
using Stripe;
using Stripe.Checkout;

namespace NetflixClone.Infrastructure.Services
{
    public class StripeService : IStripeService
    {
        private readonly StripeOptions _options;
        private readonly ILogger<StripeService> _logger;

        public StripeService(IOptions<StripeOptions> options, ILogger<StripeService> logger)
        {
            _options = options.Value;
            _logger = logger;
            StripeConfiguration.ApiKey = _options.SecretKey;
        }

        public async Task<string> CreateOrGetCustomerAsync(string userId, string email)
        {
            var service = new CustomerService();

            // Try to find existing customer by userId metadata
            try
            {
                var searchResult = await service.SearchAsync(new CustomerSearchOptions
                {
                    Query = $"metadata['userId']:'{userId}'"
                });

                if (searchResult.Data.Any())
                    return searchResult.Data.First().Id;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Customer search failed, falling back to email search.");

                var existingCustomers = await service.ListAsync(
                    new CustomerListOptions { Email = email, Limit = 1 });

                if (existingCustomers.Data.Any())
                    return existingCustomers.Data.First().Id;
            }

            // Create new customer
            var customer = await service.CreateAsync(new CustomerCreateOptions
            {
                Email = email,
                Metadata = new Dictionary<string, string> { { "userId", userId } }
            });

            return customer.Id;
        }

        public async Task<CheckoutSessionResult> CreateCheckoutSessionAsync(
            string customerId, string stripePriceId, Guid planId)
        {
            var options = new SessionCreateOptions
            {
                Customer = customerId,
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new() { Price = stripePriceId, Quantity = 1 }
                },
                Mode = "subscription",
                SuccessUrl = "http://localhost:5173/success",
                CancelUrl = "http://localhost:5173/cancel",
                Metadata = new Dictionary<string, string>
                {
                    { "planId", planId.ToString() }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return new CheckoutSessionResult(session.Id, session.Url);
        }

        public Task<StripeWebhookEvent> ConstructWebhookEventAsync(string json, string signature)
        {
            // Verify the signature — throws StripeException if invalid
            var stripeEvent = EventUtility.ConstructEvent(json, signature, _options.WebhookSecret);

            string? customerId = null;
            string? subscriptionId = null;
            string? invoiceId = null;
            string? invoicePdfUrl = null;
            decimal? amountPaid = null;
            string? currency = null;
            DateTime? periodStart = null;
            DateTime? periodEnd = null;
            var metadata = new Dictionary<string, string>();

            if (stripeEvent.Data.Object is Session session)
            {
                customerId = session.CustomerId;
                subscriptionId = session.SubscriptionId;

                if (session.Metadata != null)
                    metadata = new Dictionary<string, string>(session.Metadata);

                // Fetch subscription details from Stripe to get period dates and invoice
                if (!string.IsNullOrEmpty(subscriptionId))
                {
                    var subService = new SubscriptionService();
                    var stripeSub = subService.Get(subscriptionId);

                    // Stripe SDK removed CurrentPeriod dates from the top-level Subscription object. They are now in Items.
                    var firstItem = stripeSub.Items?.Data?.FirstOrDefault();
                    periodStart = firstItem?.CurrentPeriodStart ?? DateTime.UtcNow;
                    periodEnd = firstItem?.CurrentPeriodEnd ?? DateTime.UtcNow.AddMonths(1);

                    // Get the latest invoice for amount and PDF
                    if (!string.IsNullOrEmpty(stripeSub.LatestInvoiceId))
                    {
                        var invoiceService = new InvoiceService();
                        var stripeInvoice = invoiceService.Get(stripeSub.LatestInvoiceId);

                        invoiceId = stripeInvoice.Id;
                        invoicePdfUrl = stripeInvoice.InvoicePdf;
                        amountPaid = (decimal)stripeInvoice.AmountPaid / 100m;
                        currency = stripeInvoice.Currency;
                    }
                }
            }

            var result = new StripeWebhookEvent(
                EventType: stripeEvent.Type,
                StripeCustomerId: customerId,
                StripeSubscriptionId: subscriptionId,
                StripeInvoiceId: invoiceId,
                InvoicePdfUrl: invoicePdfUrl,
                AmountPaid: amountPaid,
                Currency: currency,
                PeriodStart: periodStart,
                PeriodEnd: periodEnd,
                Metadata: metadata
            );

            return Task.FromResult(result);
        }
    }
}
