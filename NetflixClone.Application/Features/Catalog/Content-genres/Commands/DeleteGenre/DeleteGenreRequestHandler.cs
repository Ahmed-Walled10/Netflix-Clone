using MediatR;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.DeleteGenre;

public class DeleteGenreRequestHandler : IRequestHandler<DeleteGenreRequest, bool>
{
    private readonly IBaseRepository<Genre> _genreRepo;

    public DeleteGenreRequestHandler(IBaseRepository<Genre> genreRepo)
    {
        _genreRepo = genreRepo;
    }

    public async Task<bool> Handle(DeleteGenreRequest request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepo.GetByIdAsync(request.Id, cancellationToken);

        if (genre is null)
            return false;

        await _genreRepo.DeleteAsync(genre);
        await _genreRepo.SaveChangesAsync(cancellationToken);

        return true;
    }
}
