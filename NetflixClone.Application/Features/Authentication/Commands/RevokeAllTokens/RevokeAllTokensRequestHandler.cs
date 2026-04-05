using MediatR;
using Microsoft.Extensions.Logging;
using NetflixClone.Application.Contracts.Persistence;

namespace NetflixClone.Application.Features.Authentication.Commands.RevokeAllTokens;

public class RevokeAllTokensRequestHandler : IRequestHandler<RevokeAllTokensRequest, int>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<RevokeAllTokensRequestHandler> _logger;

    public RevokeAllTokensRequestHandler(
        IRefreshTokenRepository refreshTokenRepository,
        ILogger<RevokeAllTokensRequestHandler> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    public async Task<int> Handle(RevokeAllTokensRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            throw new UnauthorizedAccessException("User ID is required.");
        }

        var revokedCount = await _refreshTokenRepository.RevokeAllActiveTokensAsync(
            request.UserId, cancellationToken);

        _logger.LogInformation(
            "Revoked {Count} active refresh tokens for user {UserId}.",
            revokedCount, request.UserId);

        return revokedCount;
    }
}
