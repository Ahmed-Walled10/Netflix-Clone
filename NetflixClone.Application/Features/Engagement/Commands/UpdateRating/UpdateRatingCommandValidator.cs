using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Engagement.Commands.UpdateRating
{
    public class UpdateRatingCommandValidator : AbstractValidator<UpdateRatingCommand>
    {
        public UpdateRatingCommandValidator()
        {
            RuleFor(x => x.RatingId)
                .NotEqual(Guid.Empty).WithMessage("RatingId must be a valid identifier.");


        }
    }
}
