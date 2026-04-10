using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.DeleteGenre
{
    public class DeleteGenreRequestValidator : AbstractValidator<DeleteGenreRequest>
    {
        public DeleteGenreRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid identifier.");

        }
    }
}
