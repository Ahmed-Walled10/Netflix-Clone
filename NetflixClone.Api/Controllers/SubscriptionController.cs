using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixClone.Application.Features.Subscriptions.Commands.SubscribePlan;
using NetflixClone.Application.Features.Subscriptions.Queries.GetMySubscription;
using System.Security.Claims;

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

        [HttpPost("Subscripe")]
        public async Task<IActionResult> Subscripe(SubscribePlanRequest subscribePlanRequest)
        {
            var result = await _mediator.Send(subscribePlanRequest);
            return Ok(result);
        }

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


    }
}
