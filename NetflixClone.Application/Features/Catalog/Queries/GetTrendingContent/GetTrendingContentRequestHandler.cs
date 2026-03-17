using AutoMapper;
using MediatR;
using NetflixClone.Application.Features.Catalog.Queries.Common;
using NetflixClone.Application.Persistence;

namespace NetflixClone.Application.Features.Catalog.Queries.GetTrendingContent
{
    public class GetTrendingContentRequestHandler : IRequestHandler<GetTrendingContentRequest, List<GetCatalogResponce>>
    {
        private readonly IContentRepository _contentRepository;
        private readonly IMapper _mapper;

        public GetTrendingContentRequestHandler(IContentRepository contentRepository, IMapper mapper)
        {
            _contentRepository = contentRepository;
            _mapper = mapper;
        }

        public async Task<List<GetCatalogResponce>> Handle(GetTrendingContentRequest request, CancellationToken cancellationToken)
        {

            var trendingEntities = await _contentRepository.GetTrendingAsync(request.Count, cancellationToken);

            var responseItems = _mapper.Map<List<GetCatalogResponce>>(trendingEntities);

            return responseItems;
        }
    }
}
