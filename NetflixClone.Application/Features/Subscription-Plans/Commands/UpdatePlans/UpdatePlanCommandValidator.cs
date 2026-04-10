using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.UpdatePlans
{
    public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
    {
        public UpdatePlanCommandValidator()
        {
            RuleFor(x => x.PlanId)
                .NotEqual(Guid.Empty).WithMessage("PlanId must be a valid identifier.");

        }
    }
}
