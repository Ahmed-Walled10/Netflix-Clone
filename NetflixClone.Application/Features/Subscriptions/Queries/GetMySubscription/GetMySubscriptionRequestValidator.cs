using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Subscriptions.Queries.GetMySubscription
{
    public class GetMySubscriptionRequestValidator : AbstractValidator<GetMySubscriptionRequest>
    {
        public GetMySubscriptionRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .MaximumLength(200).WithMessage("UserId must not exceed 200 characters.");

        }
    }
}
