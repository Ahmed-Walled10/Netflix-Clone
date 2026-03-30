using MediatR;
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

        [HttpPost]
        public async Task<IActionResult> AddPlan(AddPlanRequest addPlanRequest)
        {
            var result = await _mediator.Send(addPlanRequest);
            return Ok(result);

        }

        [HttpDelete]
        public async Task<IActionResult> DeletePlan(DeletePlanRequest deletePlanRequest)
        {
             await _mediator.Send(deletePlanRequest);
            return NoContent();

        }

        [HttpGet]
        public async Task<IActionResult> GetPlans(GetPlansRequest getPlansRequest)
        {
            var result = await _mediator.Send(getPlansRequest);
            return Ok(result);

        }


    }
}
