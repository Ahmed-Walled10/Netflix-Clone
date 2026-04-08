using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixClone.Application.Features.Subscription_Plans.Commands.AddPlans;
using NetflixClone.Application.Features.Subscription_Plans.Commands.DeletePlan;
using NetflixClone.Application.Features.Subscription_Plans.Queries.GetPlans;

namespace NetflixClone.Api.Controller
{
    [Route("api/subscription/plans")]
    [ApiController]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubscriptionPlansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpPost]
        public async Task<IActionResult> AddPlan(AddPlanRequest addPlanRequest)
        {
            var result = await _mediator.Send(addPlanRequest);
            return Ok(result);

        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlan(DeletePlanRequest deletePlanRequest)
        {
             await _mediator.Send(deletePlanRequest);
            return NoContent();

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPlans()
        {
            var request = new GetPlansRequest();
            var result = await _mediator.Send(request);
            return Ok(result);

        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePlan([FromRoute] Guid id, [FromBody] NetflixClone.Application.Features.Subscription_Plans.Commands.UpdatePlans.UpdatePlanRequest requestDto)
        {
            var command = new NetflixClone.Application.Features.Subscription_Plans.Commands.UpdatePlans.UpdatePlanCommand
            {
                PlanId = id,
                Data = new NetflixClone.Domain.Entities.Subscriptions.UpdatePlanData
                {
                    Name = requestDto.Name,
                    DisplayName = requestDto.DisplayName,
                    Price = requestDto.Price,
                    BillingPeriod = requestDto.BillingPeriod,
                    MaxProfiles = requestDto.MaxProfiles,
                    MaxVideoQuality = requestDto.MaxVideoQuality,
                    StripePriceId = requestDto.StripePriceId,
                    IsActive = requestDto.IsActive
                }
            };

            await _mediator.Send(command);
            return NoContent();
        }


    }
}
