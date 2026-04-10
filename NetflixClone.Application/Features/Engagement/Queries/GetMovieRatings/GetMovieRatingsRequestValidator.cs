using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Engagement.Queries.GetMovieRatings
{
    public class GetMovieRatingsRequestValidator : AbstractValidator<GetMovieRatingsRequest>
    {
        public GetMovieRatingsRequestValidator()
        {
            RuleFor(x => x.ContentId)
                .NotEqual(Guid.Empty).WithMessage("ContentId must be a valid identifier.");

        }
    }
}
