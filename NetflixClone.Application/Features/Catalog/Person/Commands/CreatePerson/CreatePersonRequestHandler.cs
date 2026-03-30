using AutoMapper;
using MediatR;
using NetflixClone.Application.Common.Helpers;
using NetflixClone.Application.Contracts.Persistence;
using PersonEntity = NetflixClone.Domain.Entities.Catalog.Person;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.CreatePerson;

public class CreatePersonRequestHandler : IRequestHandler<CreatePersonRequest, CreatePersonResponse>
{
    private readonly IBaseRepository<PersonEntity> _personRepo;
    private readonly IMapper                       _mapper;

    public CreatePersonRequestHandler(
        IBaseRepository<PersonEntity> personRepo,
        IMapper                       mapper)
    {
        _personRepo = personRepo;
        _mapper     = mapper;
    }

    public async Task<CreatePersonResponse> Handle(
        CreatePersonRequest request,
        CancellationToken   cancellationToken)
    {
        var person = _mapper.Map<PersonEntity>(request);

        if (string.IsNullOrWhiteSpace(person.Slug))
            person.Slug = SlugHelper.GenerateSlug(request.FullName);

        await _personRepo.AddAsync(person, cancellationToken);
        await _personRepo.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CreatePersonResponse>(person);
    }

}
