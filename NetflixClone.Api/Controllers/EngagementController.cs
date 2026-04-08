using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixClone.Application.Features.Engagement.Commands.AddRating;
using NetflixClone.Application.Features.Engagement.Commands.DeleteRating;
using NetflixClone.Application.Features.Engagement.Queries.GetMovieRatings;
using NetflixClone.Application.Features.Engagement.Queries.GetMyMovieRating;
using NetflixClone.Application.Features.Engagement.Queries.GetMyRatings;
using NetflixClone.Application.Features.Engagement.Queries.GetWatchHistory;
using NetflixClone.Application.ResourceParameters;
using System.Security.Claims;

namespace NetflixClone.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EngagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid GetProfileId()
        {
            var profileIdClaim = User.Claims.FirstOrDefault(c => c.Type == "profileId")?.Value;
            if (string.IsNullOrEmpty(profileIdClaim) || !Guid.TryParse(profileIdClaim, out var profileId))
                throw new UnauthorizedAccessException("Active profile token is required.");
            return profileId;
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpPost("content/{id}/rating")]
        public async Task<IActionResult> AddRating([FromRoute]Guid id, [FromBody] AddRatingRequest addRatingRequest)
        {
            var profileId = GetProfileId();

            addRatingRequest.ContentId = id;
            addRatingRequest.ProfileId = profileId;
            var result = await _mediator.Send(addRatingRequest);

            return CreatedAtAction(nameof(GetMyMovieRating), new { Mid = id }, result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpDelete("rating/{ratingId}")]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid ratingId)
        {
            await _mediator.Send(new DeleteRatingRequest { RatingId = ratingId });
            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("content/{id}/ratings")]
        public async Task<IActionResult> GetMovieRatings([FromRoute]Guid id,[FromQuery] RatingsResourceParameters ratingsResourceParameters)
        {
            var request = new GetMovieRatingsRequest
            {
                ContentId = id,
                RatingsResourceParameters = ratingsResourceParameters
            };

            var result=await _mediator.Send(request);
            return Ok(result);

        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("content/{Mid}/rating")]
        public async Task<IActionResult> GetMyMovieRating([FromRoute] Guid Mid)
        {
            var profileId = GetProfileId();

            var request = new GetMyMovieRatingRequest
            {
                ContentId = Mid,
                ProfileId = profileId
            };

            var result= await _mediator.Send(request);
            return Ok(result);

        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpPatch("rating/{ratingId}")]
        public async Task<IActionResult> UpdateRating([FromRoute] Guid ratingId, [FromBody] NetflixClone.Application.Features.Engagement.Commands.UpdateRating.UpdateRatingRequest requestDto)
        {
            var profileId = GetProfileId();

            var command = new NetflixClone.Application.Features.Engagement.Commands.UpdateRating.UpdateRatingCommand
            {
                RatingId = ratingId,
                ProfileId = profileId,
                Data = new NetflixClone.Domain.Entities.Engagement.UpdateRatingData
                {
                    Value = requestDto.Value,
                    Review = requestDto.Review
                }
            };

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
