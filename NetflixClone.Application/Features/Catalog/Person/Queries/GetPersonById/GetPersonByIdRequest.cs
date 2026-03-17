using MediatR;
using System;

namespace NetflixClone.Application.Features.Catalog.Person.Queries.GetPersonById
{
    public class GetPersonByIdRequest : IRequest<GetPersonByIdResponse>
    {
        public Guid Id { get; set; }

    }
}
