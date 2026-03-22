using NetflixClone.Application.ResourceParameters;
using NetflixClone.Application.Responces;
using NetflixClone.Domain.Entities.Engagement;

namespace NetflixClone.Application.Contracts.Persistence
{
    public interface IRatingRepository
    {
        /// <summary>
        /// Returns a paged list of ratings for a specific content item.
        /// </summary>
        Task<PagedResult<Rating>> GetRatingsAsync(RatingsResourceParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a paged list of all ratings submitted by a specific profile.
        /// </summary>
        Task<PagedResult<Rating>> GetMyRatingsAsync( CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the single rating a specific profile gave to a specific content item.
        /// Returns null if no rating exists.
        /// </summary>
        Task<Rating?> GetMyMovieRatingAsync( Guid contentId, CancellationToken cancellationToken = default);
    }
}
