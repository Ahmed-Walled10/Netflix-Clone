using MediatR;
using Microsoft.Extensions.Logging;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;

namespace NetflixClone.Application.Features.Authentication.Commands.RevokeToken;

public class RevokeTokenRequestHandler : IRequestHandler<RevokeTokenRequest, bool>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenGeneration _jwtTokenGeneration;
    private readonly ILogger<RevokeTokenRequestHandler> _logger;

    public RevokeTokenRequestHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenGeneration jwtTokenGeneration,
        ILogger<RevokeTokenRequestHandler> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenGeneration = jwtTokenGeneration;
        _logger = logger;
    }

    public async Task<bool> Handle(RevokeTokenRequest request, CancellationToken cancellationToken)
    {
        var tokenHash = _jwtTokenGeneration.HashToken(request.RefreshToken);
        var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (storedToken == null || storedToken.IsRevoked)
        {
            _logger.LogWarning("Attempted to revoke an invalid or already-revoked token.");
            return false;
        }

        storedToken.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(storedToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Refresh token revoked for user {UserId}.", storedToken.UserId);
        return true;
    }
}
