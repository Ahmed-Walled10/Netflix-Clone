using AutoMapper;
using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Responces;

namespace NetflixClone.Application.Features.Engagement.Queries.GetMovieRatings
{
    public class GetMovieRatingsRequestHandler : IRequestHandler<GetMovieRatingsRequest, PagedResult<GetMovieRatingsResponse>>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public GetMovieRatingsRequestHandler(
            IRatingRepository ratingRepository,
            IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetMovieRatingsResponse>> Handle(GetMovieRatingsRequest request, CancellationToken cancellationToken)
        {
            // Pass the ContentId into the resource parameters so the repo can filter
            request.RatingsResourceParameters.ContentId = request.ContentId;

            var pagedEntities = await _ratingRepository.GetRatingsAsync(request.RatingsResourceParameters, cancellationToken);

            var responseItems = _mapper.Map<List<GetMovieRatingsResponse>>(pagedEntities.Items);

            return new PagedResult<GetMovieRatingsResponse>
            {
                Items = responseItems,
                TotalCount = pagedEntities.TotalCount,
                PageNumber = pagedEntities.PageNumber,
                PageSize = pagedEntities.PageSize
            };
        }
    }
}
