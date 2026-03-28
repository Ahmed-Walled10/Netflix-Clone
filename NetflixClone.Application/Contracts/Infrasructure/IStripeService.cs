
namespace NetflixClone.Application.Contracts.Infrasructure
{
    public interface IStripeService
    {
        Task<string> CreateOrGetCustomerAsync(string userId, string email);
        Task<CheckoutSessionResult> CreateCheckoutSessionAsync(string customerId, string stripePriceId, Guid planId);
        Task<StripeWebhookEvent> ConstructWebhookEventAsync(string json, string signature);
    }

    public record CheckoutSessionResult(string SessionId, string Url);

    public record StripeWebhookEvent(
        string EventType,
        string? StripeCustomerId,
        string? StripeSubscriptionId,
        string? StripeInvoiceId,
        string? InvoicePdfUrl,
        decimal? AmountPaid,
        string? Currency,
        DateTime? PeriodStart,
        DateTime? PeriodEnd,
        Dictionary<string, string> Metadata
    );
}
