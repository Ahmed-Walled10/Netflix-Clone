using MediatR;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.UpdateGenre
{
    public class UpdateGenreCommand : IRequest<bool>
    {
        public Guid GenreId { get; set; }
        public UpdateGenreData Data { get; set; } = new();
    }
}
