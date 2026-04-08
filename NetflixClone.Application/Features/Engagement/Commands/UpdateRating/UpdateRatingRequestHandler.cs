using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Engagement;

namespace NetflixClone.Application.Features.Engagement.Commands.UpdateRating
{
    public class UpdateRatingRequestHandler : IRequestHandler<UpdateRatingCommand, bool>
    {
        private readonly IBaseRepository<Rating> _ratingBaseRepository;

        public UpdateRatingRequestHandler(IBaseRepository<Rating> ratingBaseRepository)
        {
            _ratingBaseRepository = ratingBaseRepository;
        }

        public async Task<bool> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _ratingBaseRepository.GetByIdAsync(request.RatingId);

            if (rating == null)
                throw new KeyNotFoundException($"Rating with Id {request.RatingId} not found.");

            if (rating.ProfileId != request.ProfileId)
                throw new UnauthorizedAccessException("You can only update your own ratings.");

            rating.Update(request.Data);

            await _ratingBaseRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
