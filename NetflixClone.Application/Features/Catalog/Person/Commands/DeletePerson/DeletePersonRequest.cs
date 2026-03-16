using MediatR;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.DeletePerson;

public class DeletePersonRequest : IRequest<bool>
{
    public Guid Id { get; set; }
}
