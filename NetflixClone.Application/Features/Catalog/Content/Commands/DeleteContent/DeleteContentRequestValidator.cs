using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.DeleteContent
{
    public class DeleteContentRequestValidator : AbstractValidator<DeleteContentRequest>
    {
        public DeleteContentRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid identifier.");

        }
    }
}
