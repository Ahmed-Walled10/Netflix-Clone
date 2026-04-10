using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Streaming.Queries.PlayContent
{
    public class PlayContentRequestValidator : AbstractValidator<PlayContentRequest>
    {
        public PlayContentRequestValidator()
        {
            RuleFor(x => x.ContentId)
                .NotEqual(Guid.Empty).WithMessage("ContentId must be a valid identifier.");

            When(x => x.EpisodeId != null, () => {
            RuleFor(x => x.EpisodeId)
                .NotEqual(Guid.Empty).WithMessage("EpisodeId must be a valid identifier.");
            });
            RuleFor(x => x.ProfileId)
                .NotEqual(Guid.Empty).WithMessage("ProfileId must be a valid identifier.");

        }
    }
}
