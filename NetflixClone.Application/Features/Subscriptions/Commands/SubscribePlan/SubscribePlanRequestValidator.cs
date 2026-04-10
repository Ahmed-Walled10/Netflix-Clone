using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Subscriptions.Commands.SubscribePlan
{
    public class SubscribePlanRequestValidator : AbstractValidator<SubscribePlanRequest>
    {
        public SubscribePlanRequestValidator()
        {
            RuleFor(x => x.PlanId)
                .NotEqual(Guid.Empty).WithMessage("PlanId must be a valid identifier.");


        }
    }
}
