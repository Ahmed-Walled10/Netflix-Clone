using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Person.Queries.GetPersonById
{
    public class GetPersonByIdRequestValidator : AbstractValidator<GetPersonByIdRequest>
    {
        public GetPersonByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid identifier.");

        }
    }
}
