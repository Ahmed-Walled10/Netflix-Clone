using NetflixClone.Domain.Common.Primitives;


namespace NetflixClone.Domain.Entities.Catalog;

public class Genre : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    /// <summary>URL-safe slug, e.g. "sci-fi", "romantic-comedy".</summary>
    public string Slug { get; set; } = string.Empty;

    // ── Navigation ───────────────────────────────────────────────────
    public ICollection<ContentGenre> ContentGenres { get; set; } = [];
}
