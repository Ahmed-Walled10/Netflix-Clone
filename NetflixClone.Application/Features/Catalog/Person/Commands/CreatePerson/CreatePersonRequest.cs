using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.CreatePerson;

public class CreatePersonRequest : IRequest<CreatePersonResponse>
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(220)]
    public string? Slug { get; set; }

    [StringLength(2000)]
    public string? Bio { get; set; }

    public DateOnly? BirthDate { get; set; }

    [Url]
    public string? PhotoUrl { get; set; }
}
