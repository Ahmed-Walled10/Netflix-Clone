using AutoMapper;
using MediatR;
using NetflixClone.Application.Common.Helpers;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.CreateGenre;

public class CreateGenreRequestHandler : IRequestHandler<CreateGenreRequest, CreateGenreResponse>
{
    private readonly IBaseRepository<Genre> _genreRepo;
    private readonly IMapper                _mapper;

    public CreateGenreRequestHandler(
        IBaseRepository<Genre> genreRepo,
        IMapper                mapper)
    {
        _genreRepo = genreRepo;
        _mapper    = mapper;
    }

    public async Task<CreateGenreResponse> Handle(
        CreateGenreRequest request,
        CancellationToken  cancellationToken)
    {
        var genre = _mapper.Map<Genre>(request);

        if (string.IsNullOrWhiteSpace(genre.Slug))
            genre.Slug = SlugHelper.GenerateSlug(request.Name);

        await _genreRepo.AddAsync(genre, cancellationToken);
        await _genreRepo.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CreateGenreResponse>(genre);
    }

}
