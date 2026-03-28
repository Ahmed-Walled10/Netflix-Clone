using Microsoft.EntityFrameworkCore;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Identity;
using NetflixClone.Infrastructure.Persistence;

namespace NetflixClone.Persistence.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly NetflixCloneDbContext _context;

        public ProfileRepository(NetflixCloneDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetUserWithProfilesAsync(string userId)
        {
            return await _context.Users
                .Include(u => u.Profiles)
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new KeyNotFoundException($"User with Id {userId} was not found.");
        }

        public async Task<ApplicationUser> GetUserWithSubscriptionsAsync(string userId)
        {
            return await _context.Users
                .Include(u => u.Subscriptions)
                    .ThenInclude(s => s.Plan)
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new KeyNotFoundException($"User with Id {userId} was not found.");
        }

        public async Task AddProfileAsync(Profile profile)
        {
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }
    }
}
