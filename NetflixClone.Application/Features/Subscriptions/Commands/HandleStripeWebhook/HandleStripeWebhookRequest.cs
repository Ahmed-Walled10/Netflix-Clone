using MediatR;

namespace NetflixClone.Application.Features.Subscriptions.Commands.HandleStripeWebhook
{
    public record HandleStripeWebhookRequest(string JsonPayload, string StripeSignatureHeader) : IRequest<Unit>;
}
