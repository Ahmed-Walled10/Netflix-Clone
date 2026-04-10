using FluentValidation;

namespace NetflixClone.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("NewPassword is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .MaximumLength(200).WithMessage("NewPassword must not exceed 200 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email.");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("Otp is required.")
                .Length(6).WithMessage("Otp must be exactly 6 digits long.");
        }
    }
}
