using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Contracts.Infrasructure
{
    public interface IJwtTokenGeneration
    {
        string GenerateJwtToken(ApplicationUser user, List<string> roles);

        string GenerateProfileJwtToken(ApplicationUser user, Profile profile, List<string> roles);

        /// <summary>
        /// Generates a cryptographically secure random token string for use as a refresh token.
        /// </summary>
        string GenerateRefreshToken();

        /// <summary>
        /// Computes the SHA-256 hash of a plain-text token.
        /// Only the hash is persisted; the plain token is returned to the client.
        /// </summary>
        string HashToken(string token);

        /// <summary>
        /// Gets the configured refresh token expiry duration.
        /// </summary>
        int RefreshTokenExpiryDays { get; }
    }
}
