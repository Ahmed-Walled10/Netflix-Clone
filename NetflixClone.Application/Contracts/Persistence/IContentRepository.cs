using MediatR;
using NetflixClone.Application.ResourceParameters;
using NetflixClone.Application.Responces;
using NetflixClone.Domain.Entities.Catalog;
using Stripe.Forwarding;

namespace NetflixClone.Application.Persistence;

public interface IContentRepository : IBaseRepository<Content>
{
    /// <summary>
    /// Retrieves a paginated list of Content matching the provided catalog parameters.
    /// This is the recommended approach for queries, keeping complex EF LINQ logic in the Persistence layer.
    /// </summary>
    Task<PagedResult<Content>> GetCatalogAsync(CatalogResourceParameters parameters,bool IsRequestedByAdmin, bool IsKidsMode, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves trending content based on view counts or other metrics.
    /// </summary>
    Task<IReadOnlyList<Content>> GetTrendingAsync(bool IsKidsMode, int count = 10, CancellationToken cancellationToken = default);
}
