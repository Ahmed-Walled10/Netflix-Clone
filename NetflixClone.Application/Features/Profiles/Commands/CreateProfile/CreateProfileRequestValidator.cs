using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Profiles.Commands.CreateProfile
{
    public class CreateProfileRequestValidator : AbstractValidator<CreateProfileRequest>
    {
        public CreateProfileRequestValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(20).WithMessage("Name must not exceed 20 characters.")
                .MinimumLength(1).WithMessage("Name must be at least 1 characters long.");

            RuleFor(x=>x.Age)
                .NotEmpty().WithMessage("Age is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Age must be a non-negative integer.");

            RuleFor(x => x.PinHash)
                .Length(6).WithMessage("PinHash must be exactly 6 digits long.");

            RuleFor(x => x.PreferredLanguage)
                .MaximumLength(10).WithMessage("PreferredLanguage must not exceed 10 characters.");

        }
    }
}
