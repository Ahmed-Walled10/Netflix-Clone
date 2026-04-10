using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Profiles.Commands.SwitchProfile
{
    public class SwitchProfileRequestValidator : AbstractValidator<SwitchProfileRequest>
    {
        public SwitchProfileRequestValidator()
        {

            RuleFor(x => x.ProfileId)
                .NotEqual(Guid.Empty).WithMessage("ProfileId must be a valid identifier.");

            RuleFor(x => x.Pin)
                .Length(6).WithMessage("PinHash must be exactly 6 digits long.");

        }
    }
}
