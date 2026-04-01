using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixClone.Application.Features.Subscriptions.Commands.SubscribePlan;
using NetflixClone.Application.Features.Subscriptions.Queries.GetMySubscription;
using System.Security.Claims;
using System.IO;
using NetflixClone.Application.Features.Subscriptions.Commands.HandleStripeWebhook;

namespace NetflixClone.Api.Controller
{
    [Route("api/subscription")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("Subscripe")]
        public async Task<IActionResult> Subscripe(SubscribePlanRequest subscribePlanRequest)
        {
            var result = await _mediator.Send(subscribePlanRequest);
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("my-subscription")]
        public async Task<IActionResult> GetMysubscription()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("Active profile token is required.");
            

            var request = new GetMySubscriptionRequest { UserId = userId };

            var result = await _mediator.Send(request);
            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"].ToString();

            var command = new HandleStripeWebhookRequest(json, signatureHeader);
            await _mediator.Send(command);

            return Ok();
        }
    }
}
