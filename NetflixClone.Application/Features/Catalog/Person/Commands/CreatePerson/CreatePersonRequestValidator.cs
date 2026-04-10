using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.CreatePerson
{
    public class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequest>
    {
        public CreatePersonRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.");

            RuleFor(x => x.Slug)
                .MaximumLength(200).WithMessage("Slug must not exceed 200 characters.");

            RuleFor(x => x.Bio)
                .MaximumLength(400).WithMessage("Bio must not exceed 200 characters.");


        }
    }
}
