using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Engagement.Queries.GetMyMovieRating
{
    public class GetMyMovieRatingRequestValidator : AbstractValidator<GetMyMovieRatingRequest>
    {
        public GetMyMovieRatingRequestValidator()
        {
            RuleFor(x => x.ContentId)
                .NotEqual(Guid.Empty).WithMessage("ContentId must be a valid identifier.");

        }
    }
}
