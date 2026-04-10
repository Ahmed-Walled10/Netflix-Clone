using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.CreateGenre
{
    public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
    {
        public CreateGenreRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(1).WithMessage("Name must be at least 1 character long.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Slug)
                .MaximumLength(200).WithMessage("Slug must not exceed 200 characters.");

        }
    }
}
