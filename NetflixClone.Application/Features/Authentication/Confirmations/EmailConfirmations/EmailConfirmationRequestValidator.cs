using FluentValidation;

namespace NetflixClone.Application.Features.Authentication.Confirmations.EmailConfirmations
{
    public class EmailConfirmationRequestValidator : AbstractValidator<EmailConfirmationRequest>
    {
        public EmailConfirmationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email.");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("Otp is required.")
                .Length(6).WithMessage("Otp must be exactly 6 characters long.");
        }
    }
}
