using MediatR;
using NetflixClone.Domain.Entities.Engagement;

namespace NetflixClone.Application.Features.Engagement.Commands.UpdateRating
{
    public class UpdateRatingCommand : IRequest<bool>
    {
        public Guid RatingId { get; set; }
        public Guid ProfileId { get; set; }
        public UpdateRatingData Data { get; set; } = new();
    }
}
