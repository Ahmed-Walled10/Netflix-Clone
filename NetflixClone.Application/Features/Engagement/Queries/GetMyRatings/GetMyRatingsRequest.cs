using MediatR;
using NetflixClone.Application.Responces;

namespace NetflixClone.Application.Features.Engagement.Queries.GetMyRatings
{
    public class GetMyRatingsRequest : IRequest<PagedResult<GetMyRatingsResponse>>
    {
    }
}
