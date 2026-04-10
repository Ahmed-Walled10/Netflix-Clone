using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
}
