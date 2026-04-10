using FluentValidation;

namespace NetflixClone.Application.Features.Authentication.Commands.Login
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(200).WithMessage("Password must not exceed 200 characters.");
        }
    }
}
