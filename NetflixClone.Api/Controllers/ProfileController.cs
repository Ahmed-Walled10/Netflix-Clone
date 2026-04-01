using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixClone.Application.Features.Engagement.Queries.GetMyRatings;
using NetflixClone.Application.Features.Engagement.Queries.GetWatchHistory;
using NetflixClone.Application.Features.Profiles.Commands.CreateProfile;
using NetflixClone.Application.Features.Profiles.Commands.DeleteProfile;
using NetflixClone.Application.Features.Profiles.Commands.LoginToProfile;
using NetflixClone.Application.Features.Profiles.Commands.SwitchProfile;
using NetflixClone.Application.Features.Profiles.Queries.GetProfiles;
using NetflixClone.Application.Features.Subscriptions.Queries.GetMySubscription;
using System.Security.Claims;

namespace NetflixClone.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator)
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
        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateProfileRequest createProfileRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("Please Login in first.");

            createProfileRequest.UserId = userId;

            var result = await _mediator.Send(createProfileRequest);
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProfile()
        {
            var ProfileId= GetProfileId();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("Please Login in first.");

            var request= new DeleteProfileRequest {
                ProfileId = ProfileId,
                UserId = userId
            };

            await _mediator.Send(request);
            return NoContent();
        }



        //Login profile endpoint
        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpPost("login")]
        public async Task<IActionResult> LoginProfile(LoginToProfileRequest loginToProfileRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("Please Login in first.");

            loginToProfileRequest.UserId = userId;

            var result = await _mediator.Send(loginToProfileRequest);
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpPost("switch")]
        public async Task<IActionResult> SwitchProfile(SwitchProfileRequest switchProfileRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("Please Login first.");

            switchProfileRequest.UserId = userId;

            var result = await _mediator.Send(switchProfileRequest);
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet]
        public async Task<IActionResult> GetProfiles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("Active profile token is required.");

            var request = new GetProfilesRequest
            {
                userId = userId
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        //Engagment feature
        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("watch-history")]
        public async Task<IActionResult> GetMyWatchHistory(bool ContinueWatchingOnly)
        {
            var profileId = GetProfileId();

            var request = new GetWatchHistoryRequest
            {
                ProfileId = profileId,
                ContinueWatchingOnly = ContinueWatchingOnly
            };

            var result = await _mediator.Send(request);
            return Ok(result);

        }

        //Engagment feature
        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("my-ratings")]
        public async Task<IActionResult> GetMyRatings()
        {
            var profileId = GetProfileId();

            var request = new GetMyRatingsRequest
            {
                profileId = profileId
            };

            var result = await _mediator.Send(request);
            return Ok(result);

        }



    }
}
