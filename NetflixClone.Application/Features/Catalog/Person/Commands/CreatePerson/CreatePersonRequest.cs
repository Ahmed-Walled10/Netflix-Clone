using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.CreatePerson;

public class CreatePersonRequest : IRequest<CreatePersonResponse>
{
    public string FullName { get; set; } = string.Empty;

    public string? Slug { get; set; }

    public string? Bio { get; set; }

    public DateOnly? BirthDate { get; set; }

    [Url]
    public string? PhotoUrl { get; set; }
}
