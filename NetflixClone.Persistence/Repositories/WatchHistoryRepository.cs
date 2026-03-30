using Microsoft.EntityFrameworkCore;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Responces;
using NetflixClone.Domain.Entities.Engagement;
using NetflixClone.Infrastructure.Persistence;

namespace NetflixClone.Persistence.Repositories
{
    public class WatchHistoryRepository : BaseRepository<WatchHistory>, IWatchHistoryRepository
    {
        public WatchHistoryRepository(NetflixCloneDbContext context) : base(context) { }

        public async Task<PagedResult<WatchHistory>> GetWatchHistoryAsync(
            Guid profileId,
            bool continueWatchingOnly = false,
            int pageNumber = 1,
            int pageSize = 10,
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

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<WatchHistory>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
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