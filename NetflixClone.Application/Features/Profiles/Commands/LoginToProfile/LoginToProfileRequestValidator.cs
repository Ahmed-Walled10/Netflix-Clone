using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Profiles.Commands.LoginToProfile
{
    public class LoginToProfileRequestValidator : AbstractValidator<LoginToProfileRequest>
    {
        public LoginToProfileRequestValidator()
        {

            RuleFor(x => x.ProfileId)
                .NotEmpty().WithMessage("ProfileId is required.");

            RuleFor(x => x.Pin)
                .Length(6).WithMessage("PinHash must be exactly 6 digits long.");

        }
    }
}
