using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixClone.Application.Features.Catalog.Content.Commands.UploadContentImages;
using NetflixClone.Application.Features.Catalog.Content.Commands.UploadContentVideo;
using NetflixClone.Application.Features.Catalog.Content.Queries.GetContentById;
using NetflixClone.Application.Features.Catalog.ContentGenres.Commands.CreateGenre;
using NetflixClone.Application.Features.Catalog.ContentGenres.Commands.DeleteGenre;
using NetflixClone.Application.Features.Catalog.Person.Commands.CreatePerson;
using NetflixClone.Application.Features.Catalog.Person.Commands.DeletePerson;
using NetflixClone.Application.Features.Catalog.Person.Commands.UploadPersonPhoto;
using NetflixClone.Application.Features.Catalog.Person.Queries.GetPersonById;
using NetflixClone.Application.Features.Catalog.Queries.GetAllCatalog;
using NetflixClone.Application.Features.Catalog.Queries.GetTrendingContent;
using NetflixClone.Application.Features.Catalog.Content.Commands.CreateContent;
using NetflixClone.Application.Features.Catalog.Content.Commands.DeleteContent;
using NetflixClone.Application.Features.Catalog.Content.Commands.UploadEpisodeThumbnail;
using NetflixClone.Application.Features.Streaming.Queries.PlayContent;
using System.Security.Claims;

namespace NetflixClone.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CatalogController(IMediator mediator)
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


        [HttpPost("content")]
        public async Task<IActionResult> AddContent(CreateContentRequest createContentRequest)
        {
            var result = await _mediator.Send(createContentRequest);
            return CreatedAtAction(nameof(GetContentById), new { id = result.Id }, result);
        }

        [HttpDelete("content/{id}")]
        public async Task<IActionResult> DeleteContent([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteContentRequest { Id = id });
            return NoContent();
        }

        [HttpGet("content/{id}")]
        public async Task<IActionResult> GetContentById([FromRoute] Guid id)
        {
            //add is kids mode
            var result = await _mediator.Send(new GetContentByIdRequest { Id = id });
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("content/{id}/images")]
        public async Task<IActionResult> UploadContentImage([FromRoute] Guid id, [FromForm] IFormFile? thumbnail, [FromForm] IFormFile? heroImage)
        {
            var request = new UploadContentImagesRequest { ContentId = id };

            if (thumbnail != null)
            {
                request.ThumbnailStream = thumbnail.OpenReadStream();
                request.ThumbnailFileName = thumbnail.FileName;
            }

            if (heroImage != null)
            {
                request.HeroImageStream = heroImage.OpenReadStream();
                request.HeroImageFileName = heroImage.FileName;
            }

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("content/{id}/video")]
        public async Task<IActionResult> UploadContentVideo([FromRoute] Guid id, [FromQuery] Guid? episodeId, [FromForm] IFormFile file)
        {
            var request = new UploadContentVideoRequest
            {
                ContentId = id,
                EpisodeId = episodeId,
                FileStream = file.OpenReadStream(),
                FileName = file.FileName
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("episodes/{episodeId}/thumbnail")]
        public async Task<IActionResult> UploadEpisodeThumbnail([FromRoute] Guid episodeId, [FromForm] IFormFile file)
        {
            var request = new UploadEpisodeThumbnailRequest
            {
                EpisodeId = episodeId,
                FileStream = file.OpenReadStream(),
                FileName = file.FileName
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("genres")]
        public async Task<IActionResult> AddGenre(CreateGenreRequest createGenreRequest)
        {
            var result = await _mediator.Send(createGenreRequest);
            return Ok(result);
        }

        [HttpDelete("genres/{id}")]
        public async Task<IActionResult> DeleteGenre([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteGenreRequest { Id = id });
            return NoContent();
        }

        [HttpPost("person")]
        public async Task<IActionResult> AddPerson(CreatePersonRequest createPersonRequest)
        {
            var result = await _mediator.Send(createPersonRequest);
            return CreatedAtAction(nameof(GetPersonById), new { id = result.Id }, result);
        }

        [HttpDelete("person/{id}")]
        public async Task<IActionResult> DeletePerson([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeletePersonRequest { Id = id });
            return NoContent();
        }

        [HttpPost("person/{id}/photo")]
        public async Task<IActionResult> UploadPersonImage([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            var request = new UploadPersonPhotoRequest
            {
                PersonId = id,
                FileStream = file.OpenReadStream(),
                FileName = file.FileName
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("person/{id}")]
        public async Task<IActionResult> GetPersonById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetPersonByIdRequest { Id = id });

            if (result == null)
                return NotFound();
      
            return Ok(result);
        }

        [HttpGet("content")]    
        public async Task<IActionResult> GetAllContent([FromQuery] GetCatalogRequest getCatalogRequest)
        {
            bool.TryParse(User.FindFirstValue("isKidsMode"), out var isKidsMode);
            getCatalogRequest.IsKidsMode = isKidsMode;
            var result = await _mediator.Send(getCatalogRequest);
            return Ok(result);
        }

        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingContent()
        {
            bool.TryParse(User.FindFirstValue("isKidsMode"), out var isKidsMode);
            var result = await _mediator.Send(new GetTrendingContentRequest { IsKidsMode = isKidsMode });
            return Ok(result);
        }

        //Streaimng feature
        [HttpGet("content/{Id}/play")]
        public async Task<IActionResult> PlayContent(Guid ContentId, PlayContentRequest playContentRequest )
        {
            var profileId = GetProfileId();
            playContentRequest.ContentId = ContentId;
            playContentRequest.ProfileId = profileId;

            var result = await _mediator.Send(playContentRequest);
            return Ok(result);

        }



    }
}
