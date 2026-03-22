using MediatR;
using NetflixClone.Application.ResourceParameters;
using NetflixClone.Application.Responces;

namespace NetflixClone.Application.Features.Engagement.Queries.GetMovieRatings
{
    public class GetMovieRatingsRequest : IRequest<PagedResult<GetMovieRatingsResponse>>
    {
        public Guid ContentId { get; set; }

        public RatingsResourceParameters RatingsResourceParameters { get; set; } = new();
    }
}
