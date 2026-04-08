using NetflixClone.Domain.Common.Primitives;


namespace NetflixClone.Domain.Entities.Catalog;

public class Genre : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    /// <summary>URL-safe slug, e.g. "sci-fi", "romantic-comedy".</summary>
    public string Slug { get; set; } = string.Empty;

    public ICollection<ContentGenre> ContentGenres { get; set; } = [];

    // ── Behavior ─────────────────────────────────────────────────────
    public void Update(UpdateGenreData data)
    {
        if (data.Name != null) Name = data.Name;
        if (data.Slug != null) Slug = data.Slug;
    }
}

public class UpdateGenreData
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
}
