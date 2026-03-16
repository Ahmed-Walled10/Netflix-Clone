using MediatR;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.DeleteGenre;

public class DeleteGenreRequest : IRequest<bool>
{
    public Guid Id { get; set; }
}
