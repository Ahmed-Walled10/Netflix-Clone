using MediatR;
using NetflixClone.Application.Features.Catalog.Queries.Common;

namespace NetflixClone.Application.Features.Catalog.Queries.GetTrendingContent
{
    public class GetTrendingContentRequest : IRequest<List<GetCatalogResponce>>
    {
        public int Count { get; set; } = 10;
        public bool IsRequestedByAdmin { get; set; }

        public bool IsKidsMode { get; set; }

    }
}
