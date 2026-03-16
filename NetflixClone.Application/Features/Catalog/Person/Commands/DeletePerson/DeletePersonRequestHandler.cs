using MediatR;
using NetflixClone.Application.Persistence;
using PersonEntity = NetflixClone.Domain.Entities.Catalog.Person;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.DeletePerson;

public class DeletePersonRequestHandler : IRequestHandler<DeletePersonRequest, bool>
{
    private readonly IBaseRepository<PersonEntity> _personRepo;

    public DeletePersonRequestHandler(IBaseRepository<PersonEntity> personRepo)
    {
        _personRepo = personRepo;
    }

    public async Task<bool> Handle(DeletePersonRequest request, CancellationToken cancellationToken)
    {
        var person = await _personRepo.GetByIdAsync(request.Id, cancellationToken);

        if (person is null)
            return false;

        await _personRepo.DeleteAsync(person);
        await _personRepo.SaveChangesAsync(cancellationToken);

        return true;
    }
}
