using System;
using FluentValidation;

namespace NetflixClone.Application.Features.Subscriptions.Commands.HandleStripeWebhook
{
    public class HandleStripeWebhookRequestValidator : AbstractValidator<HandleStripeWebhookRequest>
    {
        public HandleStripeWebhookRequestValidator()
        {
            RuleFor(x => x.JsonPayload)
                .NotEmpty().WithMessage("JsonPayload is required.")
                .MaximumLength(200).WithMessage("JsonPayload must not exceed 200 characters.");

            RuleFor(x => x.StripeSignatureHeader)
                .NotEmpty().WithMessage("StripeSignatureHeader is required.")
                .MaximumLength(200).WithMessage("StripeSignatureHeader must not exceed 200 characters.");

        }
    }
}
