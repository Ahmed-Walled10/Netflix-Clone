using Microsoft.EntityFrameworkCore;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.ResourceParameters;
using NetflixClone.Application.Responces;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Catalog;
using NetflixClone.Infrastructure.Persistence;

namespace NetflixClone.Persistence.Repositories
{
    public class ContentRepository : BaseRepository<Content>, IContentRepository
    {
        public ContentRepository(NetflixCloneDbContext context) : base(context) { }

        public async Task<PagedResult<Content>> GetCatalogAsync(
            CatalogResourceParameters parameters,
            bool IsRequestedByAdmin,
            bool IsKidsMode,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsNoTracking()
                .Include(c => c.ContentGenres)
                    .ThenInclude(cg => cg.Genre)
                .AsQueryable();

            // ── Filters ──────────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var search = parameters.SearchQuery.Trim().ToLower();
                query = query.Where(c =>
                    c.Title.ToLower().Contains(search) ||
                    c.Description.ToLower().Contains(search));
            }


            if (parameters.GenreIds is { Count: > 0 })
            {
                query = query.Where(c =>
                    c.ContentGenres.Any(cg => parameters.GenreIds.Contains(cg.GenreId)));
            }

            if (parameters.ContentTypes is { Count: > 0 })
            {
                query = query.Where(c => parameters.ContentTypes.Contains(c.ContentType));
            }

            if (parameters.MinRating.HasValue)
            {
                query = query.Where(c =>
                    c.TotalRatings > 0 && c.AverageRating >= parameters.MinRating.Value);
            }

            if (parameters.MaturityRatings is { Count: > 0 })
            {
                query = query.Where(c => parameters.MaturityRatings.Contains(c.MaturityRating));
            }

            if (parameters.Languages is { Count: > 0 })
            {
                query = query.Where(c => parameters.Languages.Contains(c.OriginalLanguage));
            }

            if (parameters.ReleaseYear.HasValue)
            {
                query = query.Where(c => c.ReleaseYear == parameters.ReleaseYear.Value);
            }

            if (parameters.IsOriginal.HasValue)
            {
                query = query.Where(c => c.IsOriginal == parameters.IsOriginal.Value);
            }

            if (parameters.FromDate.HasValue)
            {
                query = query.Where(c => c.CreatedAt >= parameters.FromDate.Value);
            }

            if (parameters.ToDate.HasValue)
            {
                query = query.Where(c => c.CreatedAt <= parameters.ToDate.Value);
            }

            if (IsKidsMode)
            {
                // In Kids Mode, only show content with MaturityRating of G or PG
                query = query.Where(c =>
                    c.MaturityRating == MaturityRating.G ||
                    c.MaturityRating == MaturityRating.TV_PG ||
                    c.MaturityRating == MaturityRating.TV_Y7 ||
                    c.MaturityRating == MaturityRating.PG13);
            }

            // Only show available content
            if (IsRequestedByAdmin == false)
                query = query.Where(c => c.IsAvailable);

            // ── Ordering ─────────────────────────────────────────────────
            if (parameters.OrderedByRatingDesending == true)
            {
                query = query.OrderByDescending(c => c.AverageRating);
            }
            else
            {
                query = query.OrderByDescending(c => c.CreatedAt);
            }

            // ── Paging ───────────────────────────────────────────────────
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Content>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<IReadOnlyList<Content>> GetTrendingAsync(
            bool IsKidsMode,
            int count = 10,
            CancellationToken cancellationToken = default)
        {

            var query = _dbSet.AsNoTracking()
                .Where(c => c.IsAvailable);

            if (IsKidsMode)
            {
                query = query.Where(c =>
                    c.MaturityRating == MaturityRating.G ||
                    c.MaturityRating == MaturityRating.TV_PG ||
                    c.MaturityRating == MaturityRating.TV_Y7 ||
                    c.MaturityRating == MaturityRating.PG13);
            }

            return await query
                .OrderByDescending(c => c.ViewCount)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}
