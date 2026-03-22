using AutoMapper;
using NetflixClone.Application.Features.Catalog.ContentGenres.Commands.CreateGenre;
using NetflixClone.Application.Features.Catalog.Person.Commands.CreatePerson;
using NetflixClone.Application.Features.Content.Commands.CreateContent;
using NetflixClone.Application.Features.Content.Commands.UpdateContent;
using NetflixClone.Application.Features.Engagement.Commands.AddRating;
using NetflixClone.Application.Features.Engagement.Queries.GetMovieRatings;
using NetflixClone.Application.Features.Engagement.Queries.GetMyMovieRating;
using NetflixClone.Application.Features.Engagement.Queries.GetMyRatings;
using NetflixClone.Application.Features.Engagement.Queries.GetWatchHistory;
using NetflixClone.Domain.Entities.Catalog;
using NetflixClone.Domain.Entities.Engagement;
using PersonEntity = NetflixClone.Domain.Entities.Catalog.Person;

namespace NetflixClone.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ── Catalog — Content (Commands) ──────────────────────────────────────
            CreateMap<CreateContentRequest, Content>()
                .ForMember(dest => dest.Id,             opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Seasons,         opt => opt.Ignore())
                .ForMember(dest => dest.ContentGenres,   opt => opt.Ignore())
                .ForMember(dest => dest.ContentPersons,  opt => opt.Ignore())
                .ForMember(dest => dest.WatchHistories,  opt => opt.Ignore())
                .ForMember(dest => dest.Ratings,         opt => opt.Ignore())
                .ForMember(dest => dest.ViewCount,       opt => opt.MapFrom(_ => 0L))
                .ForMember(dest => dest.TotalRatings,    opt => opt.MapFrom(_ => 0));

            CreateMap<CreateSeasonRequest, Season>()
                .ForMember(dest => dest.Id,       opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.SeriesId, opt => opt.Ignore())
                .ForMember(dest => dest.Series,   opt => opt.Ignore())
                .ForMember(dest => dest.Episodes, opt => opt.Ignore());

            CreateMap<CreateEpisodeRequest, Episode>()
                .ForMember(dest => dest.Id,       opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.SeasonId, opt => opt.Ignore())
                .ForMember(dest => dest.Season,   opt => opt.Ignore());

            CreateMap<Content, CreateContentResponse>();
            CreateMap<Content, UpdateContentResponse>();

            // ── Genre ─────────────────────────────────────────────────────────
            CreateMap<CreateGenreRequest, Genre>()
                .ForMember(dest => dest.Id,            opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.ContentGenres, opt => opt.Ignore());

            CreateMap<Genre, CreateGenreResponse>();

            // ── Person ────────────────────────────────────────────────────────
            CreateMap<CreatePersonRequest, PersonEntity>()
                .ForMember(dest => dest.Id,             opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.ContentPersons, opt => opt.Ignore());

            CreateMap<PersonEntity, CreatePersonResponse>();

            // ── Catalog — Content (Queries) ───────────────────────────────────────
            CreateMap<Content, NetflixClone.Application.Features.Catalog.Queries.Common.GetCatalogResponce>();

            CreateMap<Content, NetflixClone.Application.Features.Catalog.Content.Queries.GetContentById.GetContentByIdResponse>()
                .ForMember(dest => dest.Cast, opt => opt.MapFrom(src => src.ContentPersons));

            CreateMap<ContentPerson, NetflixClone.Application.Features.Catalog.Content.Queries.GetContentById.ContentCastDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Person != null ? src.Person.FullName : string.Empty))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Person != null ? src.Person.Slug : string.Empty))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Person != null ? src.Person.PhotoUrl : null));


            // ── Catalog — Person (Queries) ────────────────────────────────────────
            CreateMap<PersonEntity, NetflixClone.Application.Features.Catalog.Person.Queries.GetPersonById.GetPersonByIdResponse>()
                .ForMember(dest => dest.Work, opt => opt.MapFrom(src => src.ContentPersons));

            CreateMap<ContentPerson, NetflixClone.Application.Features.Catalog.Person.Queries.GetPersonById.PersonWorkDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Content != null ? src.Content.Title : string.Empty))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Content != null ? src.Content.Slug : string.Empty))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Content != null ? src.Content.ThumbnailUrl : null))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Content != null ? src.Content.AverageRating : 0));

            // ── Engagement — Rating ───────────────────────────────────────────────
            CreateMap<AddRatingRequest, Rating>()
                .ForMember(dest => dest.Id,        opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.RatedAt,   opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Profile,   opt => opt.Ignore())
                .ForMember(dest => dest.Content,   opt => opt.Ignore());

            CreateMap<Rating, AddRatingResponse>();

            CreateMap<Rating, GetMovieRatingsResponse>()
                .ForMember(dest => dest.UserId,      opt => opt.MapFrom(src => src.Profile != null ? src.Profile.Id : Guid.Empty))
                .ForMember(dest => dest.UserName,    opt => opt.MapFrom(src => src.Profile != null ? src.Profile.Name : string.Empty))
                .ForMember(dest => dest.RatingValue, opt => opt.MapFrom(src => src.Value));

            CreateMap<Rating, GetMyMovieRatingResponse>();

            CreateMap<Rating, GetMyRatingsResponse>()
                .ForMember(dest => dest.ContentTitle,        opt => opt.MapFrom(src => src.Content != null ? src.Content.Title : string.Empty))
                .ForMember(dest => dest.ContentThumbnailUrl, opt => opt.MapFrom(src => src.Content != null ? src.Content.ThumbnailUrl : null));

            // ── Engagement — WatchHistory ─────────────────────────────────────────
            CreateMap<WatchHistory, GetWatchHistoryResponse>()
                .ForMember(dest => dest.ContentTitle,        opt => opt.MapFrom(src => src.Content != null ? src.Content.Title : string.Empty))
                .ForMember(dest => dest.ContentThumbnailUrl, opt => opt.MapFrom(src => src.Content != null ? src.Content.ThumbnailUrl : null));
        }
    }
}
