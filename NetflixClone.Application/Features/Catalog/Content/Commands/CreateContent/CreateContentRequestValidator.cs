using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.CreateContent
{
    public class CreateContentRequestValidator : AbstractValidator<CreateContentRequest>
    {
        public CreateContentRequestValidator()
        {
            RuleFor(x => x.ContentType)
                .NotEmpty().WithMessage("ContentType is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
                .MinimumLength(1).WithMessage("Title must be at least 1 character long.");

            RuleFor(x => x.OriginalTitle)
                .MaximumLength(200).WithMessage("OriginalTitle must not exceed 200 characters.");

            RuleFor(x => x.Slug)
                .MaximumLength(200).WithMessage("Slug must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Tagline)
                .MaximumLength(200).WithMessage("Tagline must not exceed 200 characters.");

            RuleFor(x => x.ReleaseYear)
                .NotEmpty().WithMessage("ReleaseYear is required.");

            RuleFor(x => x.MaturityRating)
                .NotEmpty().WithMessage("MaturityRating is required.");

            RuleFor(x => x.OriginalLanguage)
                .NotEmpty().WithMessage("OriginalLanguage is required.")
                .MaximumLength(10).WithMessage("OriginalLanguage must not exceed 10 characters.");

        }
    }
}
