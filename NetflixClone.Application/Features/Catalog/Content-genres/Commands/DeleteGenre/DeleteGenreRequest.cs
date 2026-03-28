using MediatR;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.DeleteGenre;

public class DeleteGenreRequest : IRequest<Unit>
{
    public Guid Id { get; set; }
}
