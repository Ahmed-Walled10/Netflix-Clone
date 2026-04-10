using FluentValidation;

namespace NetflixClone.Application.Features.Authentication.Confirmations.ResendEmailConfirmationOtp
{
    public class ResendEmailConfirmationOtpRequestValidator : AbstractValidator<ResendEmailConfirmationOtpRequest>
    {
        public ResendEmailConfirmationOtpRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email.");
        }
    }
}
