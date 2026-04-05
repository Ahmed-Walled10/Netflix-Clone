using MediatR;

namespace NetflixClone.Application.Features.Authentication.Commands.RevokeAllTokens;

/// <summary>
/// Revokes all active refresh tokens for the current user ("log out everywhere").
/// The UserId is extracted from the JWT claims by the handler.
/// </summary>
public class RevokeAllTokensRequest : IRequest<int>
{
    /// <summary>
    /// The authenticated user's ID. Set by the controller from claims.
    /// </summary>
    public string UserId { get; set; } = string.Empty;
}
