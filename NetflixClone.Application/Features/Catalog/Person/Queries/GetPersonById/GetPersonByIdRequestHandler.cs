using AutoMapper;
using MediatR;
using NetflixClone.Application.Persistence;
using PersonEntity=NetflixClone.Domain.Entities.Catalog.Person;

namespace NetflixClone.Application.Features.Catalog.Person.Queries.GetPersonById
{
    public class GetPersonByIdRequestHandler : IRequestHandler<GetPersonByIdRequest, GetPersonByIdResponse>
    {
        private readonly IBaseRepository<PersonEntity> _personRepository;
        private readonly IMapper _mapper;

        public GetPersonByIdRequestHandler(
            IBaseRepository<PersonEntity> personRepository,
            IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<GetPersonByIdResponse> Handle(GetPersonByIdRequest request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);

            if (person == null)
            {
                throw new KeyNotFoundException($"Person with Id {request.Id} was not found.");
            }

            return _mapper.Map<GetPersonByIdResponse>(person);
        }
    }
}
