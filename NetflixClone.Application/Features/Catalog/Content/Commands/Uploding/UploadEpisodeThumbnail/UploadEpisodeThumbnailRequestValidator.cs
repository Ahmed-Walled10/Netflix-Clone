using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UploadEpisodeThumbnail
{
    public class UploadEpisodeThumbnailRequestValidator : AbstractValidator<UploadEpisodeThumbnailRequest>
    {
        public UploadEpisodeThumbnailRequestValidator()
        {
            RuleFor(x => x.EpisodeId)
                .NotEqual(Guid.Empty).WithMessage("EpisodeId must be a valid identifier.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("FileName is required.")
                .MaximumLength(200).WithMessage("FileName must not exceed 200 characters.");

        }
    }
}
