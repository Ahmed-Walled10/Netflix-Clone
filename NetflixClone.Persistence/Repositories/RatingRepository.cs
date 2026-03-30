using Microsoft.EntityFrameworkCore;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.ResourceParameters;
using NetflixClone.Application.Responces;
using NetflixClone.Domain.Entities.Engagement;
using NetflixClone.Infrastructure.Persistence;

namespace NetflixClone.Persistence.Repositories
{
    public class RatingRepository : BaseRepository<Rating>, IRatingRepository
    {
        public RatingRepository(NetflixCloneDbContext context) : base(context) { }

        public async Task<PagedResult<Rating>> GetRatingsAsync(
            RatingsResourceParameters parameters,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Ratings.AsNoTracking()
                .Include(r => r.Profile)
                .AsQueryable();

            // Filter by content
            if (parameters.ContentId.HasValue)
            {
                query = query.Where(r => r.ContentId == parameters.ContentId.Value);
            }

            // Search by review text
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var search = parameters.SearchQuery.Trim().ToLower();
                query = query.Where(r =>
                    r.Review != null && r.Review.ToLower().Contains(search));
            }

            // Date range
            if (parameters.FromDate.HasValue)
            {
                query = query.Where(r => r.RatedAt >= parameters.FromDate.Value);
            }

            if (parameters.ToDate.HasValue)
            {
                query = query.Where(r => r.RatedAt <= parameters.ToDate.Value);
            }

            // Ordering
            if (parameters.OrderedByDate == true)
            {
                query = query.OrderByDescending(r => r.RatedAt);
            }
            else if (parameters.OrderedByRatingDescending == true)
            {
                query = query.OrderByDescending(r => r.Value);
            }
            else
            {
                query = query.OrderByDescending(r => r.RatedAt);
            }

            // Paging
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Rating>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<PagedResult<Rating>> GetMyRatingsAsync(
            Guid profileId,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Ratings.AsNoTracking()
                .Where(r => r.ProfileId == profileId)
                .Include(r => r.Content)
                .OrderByDescending(r => r.RatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Rating>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Rating?> GetMyMovieRatingAsync(
            Guid contentId,
            Guid profileId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Ratings.AsNoTracking()
                .Include(r => r.Profile)
                .FirstOrDefaultAsync(r => r.ContentId == contentId && r.ProfileId == profileId, cancellationToken);
        }
    }
}
