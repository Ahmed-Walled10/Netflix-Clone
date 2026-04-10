using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Profiles.Commands.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.ProfileId)
                .NotEqual(Guid.Empty).WithMessage("ProfileId must be a valid identifier.");

        }
    }
}
