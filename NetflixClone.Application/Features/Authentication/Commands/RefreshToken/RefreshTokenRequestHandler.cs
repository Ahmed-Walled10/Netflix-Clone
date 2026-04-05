using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenGeneration _jwtTokenGeneration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RefreshTokenRequestHandler> _logger;

    public RefreshTokenRequestHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenGeneration jwtTokenGeneration,
        UserManager<ApplicationUser> userManager,
        ILogger<RefreshTokenRequestHandler> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenGeneration = jwtTokenGeneration;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        // 1. Hash the incoming token and look it up
        var tokenHash = _jwtTokenGeneration.HashToken(request.RefreshToken);
        var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (storedToken == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        // 2. Reuse detection — if this token was already consumed (rotated), revoke the entire family
        if (storedToken.IsRevoked)
        {
            _logger.LogWarning(
                "Refresh token reuse detected for user {UserId}. Revoking all tokens.",
                storedToken.UserId);

            await _refreshTokenRepository.RevokeAllActiveTokensAsync(storedToken.UserId, cancellationToken);
            throw new UnauthorizedAccessException("Refresh token has been revoked. Possible token reuse detected.");
        }

        // 3. Check expiry
        if (storedToken.IsExpired)
        {
            throw new UnauthorizedAccessException("Refresh token has expired. Please log in again.");
        }

        // 4. Verify the user is still valid
        var user = storedToken.User;
        if (user == null || !user.IsActive || user.IsSuspended)
        {
            throw new UnauthorizedAccessException("User account is not active.");
        }

        // 5. Rotate — revoke the old token
        var newPlainToken = _jwtTokenGeneration.GenerateRefreshToken();
        var newTokenHash = _jwtTokenGeneration.HashToken(newPlainToken);

        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.ReplacedByTokenHash = newTokenHash;
        await _refreshTokenRepository.UpdateAsync(storedToken);

        // 6. Create the new refresh token
        var newRefreshToken = new Domain.Entities.Identity.RefreshToken
        {
            UserId = user.Id,
            TokenHash = newTokenHash,
            DeviceInfo = storedToken.DeviceInfo, // carry forward device info
            IpAddress = storedToken.IpAddress,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtTokenGeneration.RefreshTokenExpiryDays),
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        // 7. Issue new access token
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenGeneration.GenerateJwtToken(user, roles.ToList());

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newPlainToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15)
        };
    }
}
