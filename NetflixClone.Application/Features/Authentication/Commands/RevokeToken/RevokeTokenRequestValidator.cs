using FluentValidation;

namespace NetflixClone.Application.Features.Authentication.Commands.RevokeToken
{
    public class RevokeTokenRequestValidator : AbstractValidator<RevokeTokenRequest>
    {
        public RevokeTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken is required.");
        }
    }
}
