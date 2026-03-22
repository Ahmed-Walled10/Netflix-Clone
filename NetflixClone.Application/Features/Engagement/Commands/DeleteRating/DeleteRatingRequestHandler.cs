using MediatR;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Engagement;

namespace NetflixClone.Application.Features.Engagement.Commands.DeleteRating
{
    public class DeleteRatingRequestHandler : IRequestHandler<DeleteRatingRequest, bool>
    {
        private readonly IBaseRepository<Rating> _ratingRepository;
        public DeleteRatingRequestHandler(
            IBaseRepository<Rating> ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }
        public async Task<bool> Handle(DeleteRatingRequest request, CancellationToken cancellationToken)
        {
             var rating = await _ratingRepository.GetByIdAsync(request.RatingId, cancellationToken);
            if (rating == null)
                return false;

            await _ratingRepository.DeleteAsync(rating);
            await _ratingRepository.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}
