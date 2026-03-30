using MediatR;
using System;

namespace NetflixClone.Application.Features.Catalog.Content.Queries.GetContentById
{
    public class GetContentByIdRequest : IRequest<GetContentByIdResponse>
    {
        public Guid Id { get; set; }

        public bool IsUserKid { get; set; }

    }
}
