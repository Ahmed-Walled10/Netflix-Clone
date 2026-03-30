using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using PersonEntity = NetflixClone.Domain.Entities.Catalog.Person;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.DeletePerson;

public class DeletePersonRequestHandler : IRequestHandler<DeletePersonRequest, Unit>
{
    private readonly IBaseRepository<PersonEntity> _personRepo;

    public DeletePersonRequestHandler(IBaseRepository<PersonEntity> personRepo)
    {
        _personRepo = personRepo;
    }

    public async Task<Unit> Handle(DeletePersonRequest request, CancellationToken cancellationToken)
    {
        var person = await _personRepo.GetByIdAsync(request.Id, cancellationToken);

        if (person is null)
            throw new KeyNotFoundException($"Person {request.Id} was not found.");

        await _personRepo.DeleteAsync(person, cancellationToken);
        await _personRepo.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
