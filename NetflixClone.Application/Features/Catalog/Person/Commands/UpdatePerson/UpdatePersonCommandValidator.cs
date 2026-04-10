using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.UpdatePerson
{
    public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonCommandValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEqual(Guid.Empty).WithMessage("PersonId must be a valid identifier.");

        }
    }
}
