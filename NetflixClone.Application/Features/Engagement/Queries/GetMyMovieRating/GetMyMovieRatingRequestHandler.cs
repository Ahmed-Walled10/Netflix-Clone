using AutoMapper;
using MediatR;
using NetflixClone.Application.Contracts.Persistence;

namespace NetflixClone.Application.Features.Engagement.Queries.GetMyMovieRating
{
    public class GetMyMovieRatingRequestHandler : IRequestHandler<GetMyMovieRatingRequest, GetMyMovieRatingResponse?>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public GetMyMovieRatingRequestHandler(
            IRatingRepository ratingRepository,
            IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<GetMyMovieRatingResponse?> Handle(GetMyMovieRatingRequest request, CancellationToken cancellationToken)
        {
            var rating = await _ratingRepository.GetMyMovieRatingAsync(request.ContentId, request.ProfileId, cancellationToken);

            // Return null (404) if this profile hasn't rated this content yet
            if (rating == null)
                return null;

            return _mapper.Map<GetMyMovieRatingResponse>(rating);
        }
    }
}
