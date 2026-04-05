using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Authentication.Commands.Logout
{
    public class LogoutRequest : IRequest<bool>
    {
        [Required(ErrorMessage = "Refresh token is required to logout")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
