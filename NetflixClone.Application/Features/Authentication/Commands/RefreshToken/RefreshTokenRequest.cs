using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}
