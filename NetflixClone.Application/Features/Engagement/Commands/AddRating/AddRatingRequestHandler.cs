using AutoMapper;
using MediatR;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Engagement;

namespace NetflixClone.Application.Features.Engagement.Commands.AddRating
{
    public class AddRatingRequestHandler : IRequestHandler<AddRatingRequest, AddRatingResponse>
    {
        private readonly IBaseRepository<Rating> _ratingRepository;
        private readonly IMapper _mapper;
        public AddRatingRequestHandler(
            IBaseRepository<Rating> ratingRepository,
            IMapper mapper) 
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<AddRatingResponse> Handle(AddRatingRequest request, CancellationToken cancellationToken)
        {
            var movie= await _ratingRepository.GetByIdAsync(request.MovieId, cancellationToken);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with Id {request.MovieId} was not found.");

            var rating = _mapper.Map<Rating>(request);

            await _ratingRepository.AddAsync(rating, cancellationToken);
            await _ratingRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AddRatingResponse>(rating);
        }
    }
}
