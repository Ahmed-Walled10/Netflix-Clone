using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordRequest : IRequest<bool>
    {
        public string Email { get; set; } = string.Empty;
    }
}
