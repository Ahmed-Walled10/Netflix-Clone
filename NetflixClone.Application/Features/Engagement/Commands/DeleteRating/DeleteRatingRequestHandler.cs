using MediatR;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Engagement;

namespace NetflixClone.Application.Features.Engagement.Commands.DeleteRating;

public class DeleteRatingRequestHandler : IRequestHandler<DeleteRatingRequest, Unit>
{
    private readonly IBaseRepository<Rating> _ratingRepository;

    public DeleteRatingRequestHandler(
        IBaseRepository<Rating> ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    public async Task<Unit> Handle(DeleteRatingRequest request, CancellationToken cancellationToken)
    {
        var rating = await _ratingRepository.GetByIdAsync(request.RatingId, cancellationToken);

        if (rating is null)
            throw new KeyNotFoundException($"Rating {request.RatingId} was not found.");

        await _ratingRepository.DeleteAsync(rating, cancellationToken);
        await _ratingRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
