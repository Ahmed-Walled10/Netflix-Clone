using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Authentication.Commands.RevokeToken;

public class RevokeTokenRequest : IRequest<bool>
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}
