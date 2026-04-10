using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.DeletePlan
{
    public class DeletePlanRequestValidator : AbstractValidator<DeletePlanRequest>
    {
        public DeletePlanRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid identifier.");

        }
    }
}
