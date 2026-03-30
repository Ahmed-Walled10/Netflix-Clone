using AutoMapper;
using MediatR;
using NetflixClone.Application.Common.Helpers;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Catalog;
using ContentEntity = NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.CreateContent;

public class CreateContentRequestHandler : IRequestHandler<CreateContentRequest, CreateContentResponse>
{
    // ── Dependencies ──────────────────────────────────────────────────────────
    private readonly IBaseRepository<ContentEntity> _contentRepo;
    private readonly IBaseRepository<Season>   _seasonRepo;
    private readonly IBaseRepository<Episode>  _episodeRepo;
    private readonly IBaseRepository<ContentGenre>  _contentGenreRepo;
    private readonly IBaseRepository<ContentPerson> _contentPersonRepo;
    private readonly IMapper  _mapper;

    public CreateContentRequestHandler(
        IBaseRepository<ContentEntity> contentRepo,
        IBaseRepository<Season>   seasonRepo,
        IBaseRepository<Episode>  episodeRepo,
        IBaseRepository<ContentGenre>  contentGenreRepo,
        IBaseRepository<ContentPerson> contentPersonRepo,
        IMapper  mapper)
    {
        _contentRepo       = contentRepo;
        _seasonRepo        = seasonRepo;
        _episodeRepo       = episodeRepo;
        _contentGenreRepo  = contentGenreRepo;
        _contentPersonRepo = contentPersonRepo;
        _mapper            = mapper;
    }


    public async Task<CreateContentResponse> Handle(
        CreateContentRequest request,
        CancellationToken cancellationToken)
    {
        // ── 1. Business-rule validation per ContentType ────────────────────
        ValidateByContentType(request);

        // ── 2. Map request → Content domain entity ─────────────────────────
        var content = _mapper.Map<ContentEntity>(request);

        // Auto-generate slug if the caller left it blank
        if (string.IsNullOrWhiteSpace(content.Slug))
            content.Slug = SlugHelper.GenerateSlug(request.Title, request.ReleaseYear);

        // ── 3. Persist the Content root ────────────────────────────────────
        await _contentRepo.AddAsync(content, cancellationToken);

        // ── 4. Genres (join table rows) ────────────────────────────────────
        if (request.GenreIds.Count > 0)
        {
            var genreLinks = request.GenreIds
                .Distinct()
                .Select(genreId => new ContentGenre
                {
                    ContentId = content.Id,
                    GenreId   = genreId
                });

            await _contentGenreRepo.AddRangeAsync(genreLinks, cancellationToken);
        }

        // ── 5. People (cast / crew join table rows) ────────────────────────
        if (request.Persons.Count > 0)
        {
            var personLinks = request.Persons.Select(p => new ContentPerson
            {
                ContentId     = content.Id,
                PersonId      = p.PersonId,
                Role          = p.Role,
                CharacterName = p.CharacterName
            });

            await _contentPersonRepo.AddRangeAsync(personLinks, cancellationToken);
        }

        // ── 6. Seasons + Episodes (Series / Documentary-series only) ───────
        if (request.Seasons.Count > 0)
        {
            foreach (var seasonDto in request.Seasons)
            {
                var season = _mapper.Map<Season>(seasonDto);
                season.SeriesId = content.Id;

                await _seasonRepo.AddAsync(season, cancellationToken);

                // Episodes inside this season
                if (seasonDto.Episodes.Count > 0)
                {
                    var episodes = seasonDto.Episodes.Select(epDto =>
                    {
                        var ep = _mapper.Map<Episode>(epDto);
                        ep.SeasonId = season.Id;
                        return ep;
                    });

                    await _episodeRepo.AddRangeAsync(episodes, cancellationToken);
                }
            }
        }

        // ── 7. Single commit — all changes saved together ──────────────────
        await _contentRepo.SaveChangesAsync(cancellationToken);


        // ── 8. Map to response ─────────────────────────────────────────────
        return _mapper.Map<CreateContentResponse>(content);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Enforces rules that cannot be expressed with DataAnnotations alone,
    /// because they depend on the value of ContentType.
    /// </summary>
    private static void ValidateByContentType(CreateContentRequest request)
    {
        switch (request.ContentType)
        {
            case ContentType.Movie:
            case ContentType.Documentary when request.Seasons.Count == 0:
                // Single film: must have a runtime, must NOT have seasons
                if (request.DurationMinutes is null or <= 0)
                    throw new ArgumentException(
                        "DurationMinutes is required for Movie / single-film Documentary.");

                if (request.Seasons.Count > 0)
                    throw new ArgumentException(
                        "A Movie or single-film Documentary cannot have Seasons.");
                break;

            case ContentType.Series:
            case ContentType.Documentary when request.Seasons.Count > 0:
                // Episodic content: runtime lives on each episode, not the parent
                if (request.DurationMinutes.HasValue)
                    throw new ArgumentException(
                        "DurationMinutes must be null for Series. Set it on each Episode instead.");
                break;
        }
    }

}

