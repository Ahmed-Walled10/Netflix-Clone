using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.UploadPersonPhoto
{
    public class UploadPersonPhotoRequestValidator : AbstractValidator<UploadPersonPhotoRequest>
    {
        public UploadPersonPhotoRequestValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEqual(Guid.Empty).WithMessage("PersonId must be a valid identifier.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("FileName is required.")
                .MaximumLength(200).WithMessage("FileName must not exceed 200 characters.");

        }
    }
}
