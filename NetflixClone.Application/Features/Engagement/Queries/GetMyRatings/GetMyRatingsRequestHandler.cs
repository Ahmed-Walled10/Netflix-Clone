using AutoMapper;
using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Responces;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Engagement.Queries.GetMyRatings
{
    public class GetMyRatingsRequestHandler : IRequestHandler<GetMyRatingsRequest, PagedResult<GetMyRatingsResponse>>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public GetMyRatingsRequestHandler(
            IRatingRepository ratingRepository,
            IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetMyRatingsResponse>> Handle(GetMyRatingsRequest request, CancellationToken cancellationToken)
        {
            var pagedEntities = await _ratingRepository.GetMyRatingsAsync(request.profileId, cancellationToken: cancellationToken);

            var responseItems = _mapper.Map<List<GetMyRatingsResponse>>(pagedEntities.Items);

            return new PagedResult<GetMyRatingsResponse>
            {
                Items = responseItems,
                TotalCount = pagedEntities.TotalCount,
                PageNumber = pagedEntities.PageNumber,
                PageSize = pagedEntities.PageSize
            };
        }
    }
}
