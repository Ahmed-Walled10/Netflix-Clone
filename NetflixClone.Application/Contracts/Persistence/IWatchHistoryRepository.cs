using NetflixClone.Application.Responces;
using NetflixClone.Domain.Entities.Engagement;

namespace NetflixClone.Application.Contracts.Persistence
{
    public interface IWatchHistoryRepository
    {
        /// <summary>
        /// Returns a paged list of watch history entries for the given profile.
        /// </summary>
        /// <param name="profileId">The profile whose history to fetch.</param>
        /// <param name="continueWatchingOnly">When true, only returns incomplete entries.</param>
        Task<PagedResult<WatchHistory>> GetWatchHistoryAsync(
            Guid profileId,
            bool continueWatchingOnly = false,
            CancellationToken cancellationToken = default);
    }
}
