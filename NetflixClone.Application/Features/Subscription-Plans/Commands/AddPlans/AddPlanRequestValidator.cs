using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.AddPlans
{
    public class AddPlanRequestValidator : AbstractValidator<AddPlanRequest>
    {
        public AddPlanRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.DisplayName)
                .MaximumLength(100).WithMessage("DisplayName must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.BillingPeriod)
                .IsInEnum().WithMessage("BillingPeriod must be a valid stored value.");

            RuleFor(x => x.MaxProfiles)
                .GreaterThan(0).WithMessage("MaxProfiles must be greater than zero.");

             RuleFor(x => x.MaxVideoQuality)
                .IsInEnum().WithMessage("MaxVideoQuality must be a valid stored value.");


        }
    }
}
