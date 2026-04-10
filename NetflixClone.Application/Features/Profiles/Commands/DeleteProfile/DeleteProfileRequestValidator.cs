using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Profiles.Commands.DeleteProfile
{
    public class DeleteProfileRequestValidator : AbstractValidator<DeleteProfileRequest>
    {
        public DeleteProfileRequestValidator()
        {

            RuleFor(x => x.ProfileId)
                .NotEqual(Guid.Empty).WithMessage("ProfileId must be a valid identifier.");

        }
    }
}
