using AutoMapper;
using NetflixClone.Application.Features.Catalog.ContentGenres.Commands.CreateGenre;
using NetflixClone.Application.Features.Catalog.Person.Commands.CreatePerson;
using NetflixClone.Application.Features.Content.Commands.CreateContent;
using NetflixClone.Application.Features.Content.Commands.UpdateContent;
using NetflixClone.Domain.Entities.Catalog;
using PersonEntity = NetflixClone.Domain.Entities.Catalog.Person;

namespace NetflixClone.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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
        }
    }
}
