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
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpPost("content")]
        public async Task<IActionResult> AddContent(CreateContentRequest createContentRequest)
        {
            var result = await _mediator.Send(createContentRequest);
            return CreatedAtAction(nameof(GetContentById), new { id = result.Id }, result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpPatch("content/{id}")]
        public async Task<IActionResult> UpdateContent([FromRoute]Guid id, [FromBody] NetflixClone.Application.Features.Catalog.Content.Commands.UpdateContent.UpdateContentRequest requestDto)
        {
            var command = new NetflixClone.Application.Features.Catalog.Content.Commands.UpdateContent.UpdateContentCommand
            {
                ContentId = id,
                Data = new NetflixClone.Domain.Entities.Catalog.UpdateContentData
                {
                    ContentType = requestDto.ContentType,
                    Title = requestDto.Title,
                    OriginalTitle = requestDto.OriginalTitle,
                    Slug = requestDto.Slug,
                    Description = requestDto.Description,
                    Tagline = requestDto.Tagline,
                    ReleaseYear = requestDto.ReleaseYear,
                    EndYear = requestDto.EndYear,
                    DurationMinutes = requestDto.DurationMinutes,
                    MaturityRating = requestDto.MaturityRating,
                    OriginalLanguage = requestDto.OriginalLanguage,
                    VideoUrl = requestDto.VideoUrl,
                    CloudinaryPublicId = requestDto.CloudinaryPublicId,
                    TrailerUrl = requestDto.TrailerUrl,
                    ThumbnailUrl = requestDto.ThumbnailUrl,
                    HeroImageUrl = requestDto.HeroImageUrl,
                    IsAvailable = requestDto.IsAvailable,
                    IsOriginal = requestDto.IsOriginal
                }
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpDelete("content/{id}")]
        public async Task<IActionResult> DeleteContent([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteContentRequest { Id = id });
            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("content/{id}")]
        public async Task<IActionResult> GetContentById([FromRoute] Guid id)
        {

            bool.TryParse(User.FindFirstValue("isKidsMode"), out var isKidsMode);
            var result = await _mediator.Send(new GetContentByIdRequest {
                Id = id,
                IsUserKid= isKidsMode
            });


            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
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

        [Authorize(Roles = "SuperAdmin,ContentManager")]
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

        [Authorize(Roles = "SuperAdmin,ContentManager")]
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

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpPost("genres")]
        public async Task<IActionResult> AddGenre(CreateGenreRequest createGenreRequest)
        {
            var result = await _mediator.Send(createGenreRequest);
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpDelete("genres/{id}")]
        public async Task<IActionResult> DeleteGenre([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteGenreRequest { Id = id });
            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpPatch("genres/{id}")]
        public async Task<IActionResult> UpdateGenre([FromRoute] Guid id, [FromBody] NetflixClone.Application.Features.Catalog.ContentGenres.Commands.UpdateGenre.UpdateGenreRequest requestDto)
        {
            var command = new NetflixClone.Application.Features.Catalog.ContentGenres.Commands.UpdateGenre.UpdateGenreCommand
            {
                GenreId = id,
                Data = new NetflixClone.Domain.Entities.Catalog.UpdateGenreData
                {
                    Name = requestDto.Name,
                    Slug = requestDto.Slug
                }
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpPost("person")]
        public async Task<IActionResult> AddPerson(CreatePersonRequest createPersonRequest)
        {
            var result = await _mediator.Send(createPersonRequest);
            return CreatedAtAction(nameof(GetPersonById), new { id = result.Id }, result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpDelete("person/{id}")]
        public async Task<IActionResult> DeletePerson([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeletePersonRequest { Id = id });
            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
        [HttpPatch("person/{id}")]
        public async Task<IActionResult> UpdatePerson([FromRoute] Guid id, [FromBody] NetflixClone.Application.Features.Catalog.Person.Commands.UpdatePerson.UpdatePersonRequest requestDto)
        {
            var command = new NetflixClone.Application.Features.Catalog.Person.Commands.UpdatePerson.UpdatePersonCommand
            {
                PersonId = id,
                Data = new NetflixClone.Domain.Entities.Catalog.UpdatePersonData
                {
                    FullName = requestDto.FullName,
                    Slug = requestDto.Slug,
                    Bio = requestDto.Bio,
                    BirthDate = requestDto.BirthDate,
                    PhotoUrl = requestDto.PhotoUrl
                }
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin,ContentManager")]
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

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("person/{id}")]
        public async Task<IActionResult> GetPersonById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetPersonByIdRequest { Id = id });

            if (result == null)
                return NotFound();
      
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("content")]    
        public async Task<IActionResult> GetAllContent([FromQuery] GetCatalogRequest getCatalogRequest)
        {
            var profileId = GetProfileId();
            bool isAdmin = false;
            if (User.IsInRole("SuperAdmin") || User.IsInRole("ContentManager"))
                 isAdmin = true;

            bool.TryParse(User.FindFirstValue("isKidsMode"), out var isKidsMode);
            getCatalogRequest.IsKidsMode = isKidsMode;
            getCatalogRequest.IsRequestedByAdmin = isAdmin;
            var result = await _mediator.Send(getCatalogRequest);
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingContent()
        {
            var profileId = GetProfileId();
            bool.TryParse(User.FindFirstValue("isKidsMode"), out var isKidsMode);
            var result = await _mediator.Send(new GetTrendingContentRequest { IsKidsMode = isKidsMode });
            return Ok(result);
        }

        //Streaimng feature
        [Authorize(Roles = "SuperAdmin,ContentManager,Subscriber")]
        [HttpGet("content/{id}/play")]
        public async Task<IActionResult> PlayContent([FromRoute] Guid id, [FromQuery] PlayContentRequest playContentRequest )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("Please Login first.");

            var profileId = GetProfileId();
            playContentRequest.ContentId = id;
            playContentRequest.ProfileId = profileId;
            playContentRequest.UserId = userId;

            var result = await _mediator.Send(playContentRequest);
            return Ok(result);

        }



    }
}
