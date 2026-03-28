using AutoMapper;
using MediatR;
using NetflixClone.Application.Features.Catalog.Queries.Common;
using NetflixClone.Application.Persistence;
using NetflixClone.Application.Responces;
using Stripe;

namespace NetflixClone.Application.Features.Catalog.Queries.GetAllCatalog
{
    public class GetCatalogRequestHandler : IRequestHandler<GetCatalogRequest, PagedResult<GetCatalogResponce>>
    {
        private readonly IContentRepository _contentRepository;
        private readonly IMapper _mapper;

        public GetCatalogRequestHandler(IContentRepository contentRepository, IMapper mapper)
        {
            _contentRepository = contentRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetCatalogResponce>> Handle(GetCatalogRequest request, CancellationToken cancellationToken)
        {

            var pagedEntities = await _contentRepository.GetCatalogAsync(request.CatalogResourceParameters,request.IsRequestedByAdmin, cancellationToken);

            var responseItems = _mapper.Map<List<GetCatalogResponce>>(pagedEntities.Items);

            return new PagedResult<GetCatalogResponce>
            {
                Items = responseItems,
                TotalCount = pagedEntities.TotalCount,
                PageNumber = pagedEntities.PageNumber,
                PageSize = pagedEntities.PageSize
            };
        }
    }
}
