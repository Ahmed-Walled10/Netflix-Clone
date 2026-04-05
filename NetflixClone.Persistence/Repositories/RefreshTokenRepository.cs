using Microsoft.EntityFrameworkCore;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Identity;
using NetflixClone.Infrastructure.Persistence;

namespace NetflixClone.Persistence.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(NetflixCloneDbContext context) : base(context) { }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default)
    {
        return await _dbSet
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, ct);
    }

    public async Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .Where(rt => rt.UserId == userId
                         && rt.RevokedAt == null
                         && rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(ct);
    }

    public async Task<int> RevokeAllActiveTokensAsync(string userId, CancellationToken ct = default)
    {
        var activeTokens = await GetActiveTokensByUserIdAsync(userId, ct);

        foreach (var token in activeTokens)
        {
            token.RevokedAt = DateTime.UtcNow;
        }

        return await _context.SaveChangesAsync(ct);
    }
}
