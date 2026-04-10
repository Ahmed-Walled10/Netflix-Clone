using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Authentication.Commands.Logout
{
    public class LogoutRequest : IRequest<bool>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
