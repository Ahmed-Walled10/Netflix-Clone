using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Content.Queries.GetContentById
{
    public class GetContentByIdRequestValidator : AbstractValidator<GetContentByIdRequest>
    {
        public GetContentByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid identifier.");

        }
    }
}
