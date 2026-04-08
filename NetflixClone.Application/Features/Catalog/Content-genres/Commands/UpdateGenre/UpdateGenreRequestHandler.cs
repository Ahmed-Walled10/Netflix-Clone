using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using GenreEntity = NetflixClone.Domain.Entities.Catalog.Genre;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.UpdateGenre
{
    public class UpdateGenreRequestHandler : IRequestHandler<UpdateGenreCommand, bool>
    {
        private readonly IBaseRepository<GenreEntity> _genreBaseRepository;

        public UpdateGenreRequestHandler(IBaseRepository<GenreEntity> genreBaseRepository)
        {
            _genreBaseRepository = genreBaseRepository;
        }

        public async Task<bool> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _genreBaseRepository.GetByIdAsync(request.GenreId);

            if (genre == null)
                throw new KeyNotFoundException($"Genre with Id {request.GenreId} not found.");

            genre.Update(request.Data);

            await _genreBaseRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
