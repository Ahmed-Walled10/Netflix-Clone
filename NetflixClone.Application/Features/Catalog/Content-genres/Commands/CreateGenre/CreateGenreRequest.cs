using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.CreateGenre;

public class CreateGenreRequest : IRequest<CreateGenreResponse>
{
    public string Name { get; set; } = string.Empty;

    public string? Slug { get; set; }
}
