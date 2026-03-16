namespace NetflixClone.Application.Persistence;

/// <summary>
/// Generic async repository contract. Every entity gets its own
/// typed registration (IBaseRepository&lt;Content&gt;, IBaseRepository&lt;Season&gt;, …).
/// The concrete implementation lives in the Persistence layer.
/// </summary>
public interface IBaseRepository<T> where T : class
{
    // ── Reads ─────────────────────────────────────────────────────────────────
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    // ── Writes ────────────────────────────────────────────────────────────────
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);

    // ── Commit ────────────────────────────────────────────────────────────────
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

