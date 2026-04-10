using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.DeletePerson
{
    public class DeletePersonRequestValidator : AbstractValidator<DeletePersonRequest>
    {
        public DeletePersonRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid identifier.");

        }
    }
}
