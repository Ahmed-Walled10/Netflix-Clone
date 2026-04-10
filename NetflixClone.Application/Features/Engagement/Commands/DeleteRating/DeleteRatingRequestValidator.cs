using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Engagement.Commands.DeleteRating
{
    public class DeleteRatingRequestValidator : AbstractValidator<DeleteRatingRequest>
    {
        public DeleteRatingRequestValidator()
        {
            RuleFor(x => x.RatingId)
                .NotEqual(Guid.Empty).WithMessage("RatingId must be a valid identifier.");

        }
    }
}
