using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Authentication.Commands.RevokeToken;

public class RevokeTokenRequest : IRequest<bool>
{
    public string RefreshToken { get; set; } = string.Empty;

}
