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
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds an existing watch-history record for a profile + content + optional episode.
        /// Returns null if the user has never watched this content/episode on this profile.
        /// </summary>
        Task<WatchHistory?> GetByProfileAndContentAsync(
            Guid profileId,
            Guid contentId,
            Guid? episodeId = null,
            CancellationToken cancellationToken = default);
    }
}
