using MediatR;
using NetflixClone.Application.Features.Catalog.Queries.Common;
using NetflixClone.Application.ResourceParameters;
using NetflixClone.Application.Responces;

namespace NetflixClone.Application.Features.Catalog.Queries.GetAllCatalog
{
    public class GetCatalogRequest : IRequest<PagedResult<GetCatalogResponce>>
    {
            public CatalogResourceParameters CatalogResourceParameters { get; set; } = new();

            public bool IsRequestedByAdmin { get; set; } = false;

            public bool IsKidsMode { get; set; }
    }
}
