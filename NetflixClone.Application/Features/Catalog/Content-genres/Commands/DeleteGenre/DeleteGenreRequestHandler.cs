using MediatR;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.DeleteGenre;

public class DeleteGenreRequestHandler : IRequestHandler<DeleteGenreRequest, Unit>
{
    private readonly IBaseRepository<Genre> _genreRepo;

    public DeleteGenreRequestHandler(IBaseRepository<Genre> genreRepo)
    {
        _genreRepo = genreRepo;
    }

    public async Task<Unit> Handle(DeleteGenreRequest request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepo.GetByIdAsync(request.Id, cancellationToken);

        if (genre is null)
            throw new KeyNotFoundException($"Genre {request.Id} was not found.");

        await _genreRepo.DeleteAsync(genre, cancellationToken);
        await _genreRepo.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
