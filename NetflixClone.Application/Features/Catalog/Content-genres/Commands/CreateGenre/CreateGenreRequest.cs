using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Catalog.ContentGenres.Commands.CreateGenre;

public class CreateGenreRequest : IRequest<CreateGenreResponse>
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [StringLength(120)]
    public string? Slug { get; set; }
}
