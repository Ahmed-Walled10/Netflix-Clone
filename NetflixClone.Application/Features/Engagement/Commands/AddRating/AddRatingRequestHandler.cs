using AutoMapper;
using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Engagement;

namespace NetflixClone.Application.Features.Engagement.Commands.AddRating
{
    public class AddRatingRequestHandler : IRequestHandler<AddRatingRequest, AddRatingResponse>
    {
        private readonly IBaseRepository<Rating> _ratingRepository;
        private readonly IBaseRepository<NetflixClone.Domain.Entities.Catalog.Content> _contentRepository;
        private readonly IMapper _mapper;
        public AddRatingRequestHandler(
            IBaseRepository<Rating> ratingRepository,
            IBaseRepository<NetflixClone.Domain.Entities.Catalog.Content> contentRepository,
            IMapper mapper) 
        {
            _ratingRepository = ratingRepository;
            _contentRepository = contentRepository;
            _mapper = mapper;
        }

        public async Task<AddRatingResponse> Handle(AddRatingRequest request, CancellationToken cancellationToken)
        {
            var movie = await _contentRepository.GetByIdAsync(request.ContentId, cancellationToken);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with Id {request.ContentId} was not found.");

            var rating = _mapper.Map<Rating>(request);

            await _ratingRepository.AddAsync(rating, cancellationToken);

            // Recalculate stored Content stats
            var oldCount = movie.TotalRatings;
            movie.TotalRatings = oldCount + 1;
            movie.AverageRating = (movie.AverageRating * oldCount + rating.Value) / movie.TotalRatings;
            await _contentRepository.UpdateAsync(movie);

            await _ratingRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AddRatingResponse>(rating);
        }
    }
}
