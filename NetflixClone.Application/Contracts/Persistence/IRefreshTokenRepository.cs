using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Contracts.Persistence;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
    /// <summary>
    /// Finds a refresh token by its SHA-256 hash.
    /// This is the primary lookup on every /auth/refresh call.
    /// </summary>
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default);

    /// <summary>
    /// Returns all non-revoked, non-expired tokens for a user.
    /// Used by "revoke all" / "log out everywhere".
    /// </summary>
    Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Bulk-revokes every active token for a user.
    /// Returns the number of tokens revoked.
    /// </summary>
    Task<int> RevokeAllActiveTokensAsync(string userId, CancellationToken ct = default);
}
