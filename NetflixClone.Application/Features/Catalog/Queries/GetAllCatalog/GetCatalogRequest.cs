using MediatR;
using NetflixClone.Application.Features.Catalog.Queries.Common;
using NetflixClone.Application.ResourceParameters;
using NetflixClone.Application.Responces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Features.Catalog.Queries.GetAllCatalog
{
    public class GetCatalogRequest : IRequest<PagedResult<GetCatalogResponce>>
    {
            public CatalogResourceParameters CatalogResourceParameters { get; set; }
            public bool IsRequestedByAdmin { get; set; }

            public bool IsKidsMode { get; set; }
    }
}
