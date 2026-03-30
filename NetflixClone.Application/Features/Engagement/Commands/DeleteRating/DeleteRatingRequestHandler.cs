using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Engagement;
using ContentEntity = NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Engagement.Commands.DeleteRating;

public class DeleteRatingRequestHandler : IRequestHandler<DeleteRatingRequest, Unit>
{
    private readonly IBaseRepository<Rating> _ratingRepository;
    private readonly IBaseRepository<ContentEntity> _contentRepository;

    public DeleteRatingRequestHandler(
        IBaseRepository<Rating> ratingRepository,
        IBaseRepository<ContentEntity> contentRepository)
    {
        _ratingRepository = ratingRepository;
        _contentRepository = contentRepository;
    }

    public async Task<Unit> Handle(DeleteRatingRequest request, CancellationToken cancellationToken)
    {
        var rating = await _ratingRepository.GetByIdAsync(request.RatingId, cancellationToken);

        if (rating is null)
            throw new KeyNotFoundException($"Rating {request.RatingId} was not found.");

        // Update stored Content stats
        var content = await _contentRepository.GetByIdAsync(rating.ContentId, cancellationToken);
        if (content is not null)
        {
            var oldCount = content.TotalRatings;
            if (oldCount <= 1)
            {
                content.TotalRatings = 0;
                content.AverageRating = 0;
            }
            else
            {
                content.TotalRatings = oldCount - 1;
                content.AverageRating = (content.AverageRating * oldCount - rating.Value) / content.TotalRatings;
            }
            await _contentRepository.UpdateAsync(content);
        }

        await _ratingRepository.DeleteAsync(rating, cancellationToken);
        await _ratingRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
