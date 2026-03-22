using MediatR;
using NetflixClone.Application.Responces;

namespace NetflixClone.Application.Features.Engagement.Queries.GetWatchHistory
{
    public class GetWatchHistoryRequest : IRequest<PagedResult<GetWatchHistoryResponse>>
    {
        public Guid ProfileId { get; set; }
        public bool ContinueWatchingOnly { get; set; } = false;
    }
}
