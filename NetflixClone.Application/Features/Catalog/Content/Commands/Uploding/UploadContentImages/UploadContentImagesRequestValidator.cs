using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UploadContentImages
{
    public class UploadContentImagesRequestValidator : AbstractValidator<UploadContentImagesRequest>
    {
        public UploadContentImagesRequestValidator()
        {
            RuleFor(x => x.ContentId)
                .NotEqual(Guid.Empty).WithMessage("ContentId must be a valid identifier.");

            RuleFor(x => x.ThumbnailFileName)
                .MaximumLength(100).WithMessage("ThumbnailFileName must not exceed 100 characters.");

            RuleFor(x => x.HeroImageFileName)
                .MaximumLength(100).WithMessage("HeroImageFileName must not exceed 100 characters.");

        }
    }
}
