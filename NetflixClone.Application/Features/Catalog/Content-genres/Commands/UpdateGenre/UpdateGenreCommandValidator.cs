using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(x => x.GenreId)
                .NotEqual(Guid.Empty).WithMessage("GenreId must be a valid identifier.");

        }
    }
}
