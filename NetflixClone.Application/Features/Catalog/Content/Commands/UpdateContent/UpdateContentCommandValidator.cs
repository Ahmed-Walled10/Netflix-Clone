using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UpdateContent
{
    public class UpdateContentCommandValidator : AbstractValidator<UpdateContentCommand>
    {
        public UpdateContentCommandValidator()
        {
            RuleFor(x => x.ContentId)
                .NotEqual(Guid.Empty).WithMessage("ContentId must be a valid identifier.");

        }
    }
}
