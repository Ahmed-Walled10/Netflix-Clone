using MediatR;

namespace NetflixClone.Application.Features.Engagement.Commands.DeleteRating;

public class DeleteRatingRequest : IRequest<Unit>
{
    public Guid RatingId { get; set; }
}
