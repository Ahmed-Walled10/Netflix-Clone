using MediatR;

namespace NetflixClone.Application.Features.Engagement.Queries.GetMyMovieRating
{
    public class GetMyMovieRatingRequest : IRequest<GetMyMovieRatingResponse?>
    {
        public Guid ContentId { get; set; }
        public Guid ProfileId { get; set; }
    }
}
