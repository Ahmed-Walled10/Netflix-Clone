using FluentValidation;

namespace NetflixClone.Application.Features.Authentication.Commands.Logout
{
    public class LogoutRequestValidator : AbstractValidator<LogoutRequest>
    {
        public LogoutRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken is required.");
        }
    }
}
