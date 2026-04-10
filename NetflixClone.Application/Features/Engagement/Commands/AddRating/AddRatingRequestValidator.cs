using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Engagement.Commands.AddRating
{
    public class AddRatingRequestValidator : AbstractValidator<AddRatingRequest>
    {
        public AddRatingRequestValidator()
        {
            RuleFor(x => x.ContentId)
                .NotEqual(Guid.Empty).WithMessage("ContentId must be a valid identifier.");

            RuleFor(x => x.Review)
                .MaximumLength(200).WithMessage("Review must not exceed 200 characters.");

            RuleFor(x=>x.Value)
                .NotEmpty().WithMessage("Rating value is required.")
                .InclusiveBetween(1, 5).WithMessage("Rating value must be between 1 and 5.");

        }
    }
}
