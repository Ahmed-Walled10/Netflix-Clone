using MediatR;
using Microsoft.Extensions.Logging;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;

namespace NetflixClone.Application.Features.Authentication.Commands.Logout
{
    public class LogoutRequestHandler : IRequestHandler<LogoutRequest, bool>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtTokenGeneration _jwtTokenGeneration;
        private readonly ILogger<LogoutRequestHandler> _logger;

        public LogoutRequestHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IJwtTokenGeneration jwtTokenGeneration,
            ILogger<LogoutRequestHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jwtTokenGeneration = jwtTokenGeneration;
            _logger = logger;
        }

        public async Task<bool> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var tokenHash = _jwtTokenGeneration.HashToken(request.RefreshToken);
                var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

                if (storedToken == null || storedToken.IsRevoked)
                {
                    _logger.LogWarning("Logout attempted with invalid or already-revoked refresh token.");
                    return false;
                }

                storedToken.RevokedAt = DateTime.UtcNow;
                await _refreshTokenRepository.UpdateAsync(storedToken);
                await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("User {UserId} logged out. Refresh token revoked.", storedToken.UserId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed.");
                return false;
            }
        }
    }
}
