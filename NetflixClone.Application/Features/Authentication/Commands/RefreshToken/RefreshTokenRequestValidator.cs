using FluentValidation;

namespace NetflixClone.Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken is required.");
        }
    }
}
