using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UploadContentVideo
{
    public class UploadContentVideoRequestValidator : AbstractValidator<UploadContentVideoRequest>
    {
        public UploadContentVideoRequestValidator()
        {
            RuleFor(x => x.ContentId)
                .NotEqual(Guid.Empty).WithMessage("ContentId must be a valid identifier.");

            When(x => x.EpisodeId != null, () => {
            RuleFor(x => x.EpisodeId)
                .NotEqual(Guid.Empty).WithMessage("EpisodeId must be a valid identifier.");
            });

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("FileName is required.")
                .MaximumLength(200).WithMessage("FileName must not exceed 200 characters.");

        }
    }
}
