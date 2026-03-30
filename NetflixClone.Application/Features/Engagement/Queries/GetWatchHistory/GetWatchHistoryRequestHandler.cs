using AutoMapper;
using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Responces;

namespace NetflixClone.Application.Features.Engagement.Queries.GetWatchHistory
{
    public class GetWatchHistoryRequestHandler : IRequestHandler<GetWatchHistoryRequest, PagedResult<GetWatchHistoryResponse>>
    {
        private readonly IWatchHistoryRepository _watchHistoryRepository;
        private readonly IMapper _mapper;

        public GetWatchHistoryRequestHandler(
            IWatchHistoryRepository watchHistoryRepository,
            IMapper mapper)
        {
            _watchHistoryRepository = watchHistoryRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetWatchHistoryResponse>> Handle(GetWatchHistoryRequest request, CancellationToken cancellationToken)
        {
            var pagedEntities = await _watchHistoryRepository.GetWatchHistoryAsync(
                request.ProfileId,
                request.ContinueWatchingOnly,
                cancellationToken: cancellationToken);

            var responseItems = _mapper.Map<List<GetWatchHistoryResponse>>(pagedEntities.Items);

            return new PagedResult<GetWatchHistoryResponse>
            {
                Items = responseItems,
                TotalCount = pagedEntities.TotalCount,
                PageNumber = pagedEntities.PageNumber,
                PageSize = pagedEntities.PageSize
            };
        }
    }
}
