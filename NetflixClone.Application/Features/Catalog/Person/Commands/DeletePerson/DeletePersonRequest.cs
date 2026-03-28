using MediatR;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.DeletePerson;

public class DeletePersonRequest : IRequest<Unit>
{
    public Guid Id { get; set; }
}
