using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordRequest : IRequest<bool>
    {
        public string NewPassword { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
    }
}
