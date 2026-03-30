using Microsoft.EntityFrameworkCore;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Subscriptions;
using NetflixClone.Infrastructure.Persistence;

namespace NetflixClone.Persistence.Repositories
{
    public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(NetflixCloneDbContext context) : base(context) { }

        public async Task<Subscription?> GetActiveByUserIdAsync(string userId)
        {
            return await _context.Subscriptions
                .Include(s => s.Plan)
                .FirstOrDefaultAsync(s =>
                    s.UserId == userId &&
                    (s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.PastDue) &&
                    s.CurrentPeriodEnd > DateTime.UtcNow);
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }
    }
}
