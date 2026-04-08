using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using PersonEntity = NetflixClone.Domain.Entities.Catalog.Person;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.UpdatePerson
{
    public class UpdatePersonRequestHandler : IRequestHandler<UpdatePersonCommand, bool>
    {
        private readonly IBaseRepository<PersonEntity> _personBaseRepository;

        public UpdatePersonRequestHandler(IBaseRepository<PersonEntity> personBaseRepository)
        {
            _personBaseRepository = personBaseRepository;
        }

        public async Task<bool> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _personBaseRepository.GetByIdAsync(request.PersonId);

            if (person == null)
                throw new KeyNotFoundException($"Person with Id {request.PersonId} not found.");

            person.Update(request.Data);

            await _personBaseRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
