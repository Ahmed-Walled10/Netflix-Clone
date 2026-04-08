using MediatR;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.UpdatePerson
{
    public class UpdatePersonCommand : IRequest<bool>
    {
        public Guid PersonId { get; set; }
        public UpdatePersonData Data { get; set; } = new();
    }
}
