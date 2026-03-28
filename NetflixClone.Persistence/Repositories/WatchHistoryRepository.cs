using Microsoft.EntityFrameworkCore;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Responces;
using NetflixClone.Domain.Entities.Engagement;
using NetflixClone.Infrastructure.Persistence;

namespace NetflixClone.Persistence.Repositories
{
    public class WatchHistoryRepository : IWatchHistoryRepository
    {
        private readonly NetflixCloneDbContext _context;

        public WatchHistoryRepository(NetflixCloneDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<WatchHistory>> GetWatchHistoryAsync(
            Guid profileId,
            bool continueWatchingOnly = false,
            CancellationToken cancellationToken = default)
        {
            var query = _context.WatchHistories.AsNoTracking()
                .Include(wh => wh.Content)
                .Where(wh => wh.ProfileId == profileId);

            if (continueWatchingOnly)
            {
                // Only incomplete entries — powers the "Continue Watching" row
                query = query.Where(wh => !wh.IsCompleted);
            }

            query = query.OrderByDescending(wh => wh.WatchedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.ToListAsync(cancellationToken);

            return new PagedResult<WatchHistory>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = 1,
                PageSize = totalCount > 0 ? totalCount : 10
            };
        }

        public async Task<WatchHistory?> GetByProfileAndContentAsync(
            Guid profileId,
            Guid contentId,
            Guid? episodeId = null,
            CancellationToken cancellationToken = default)
        {
            return await _context.WatchHistories
                .FirstOrDefaultAsync(wh =>
                    wh.ProfileId == profileId &&
                    wh.ContentId == contentId &&
                    wh.EpisodeId == episodeId,
                    cancellationToken);
        }
    }
}