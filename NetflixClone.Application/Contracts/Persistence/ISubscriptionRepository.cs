using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Application.Contracts.Persistence
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetActiveByUserIdAsync(string userId);
        Task AddInvoiceAsync(Invoice invoice);
    }
}
