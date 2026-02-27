namespace NetflixClone.Domain.Entities.Catalog;

public class ContentGenre
{
    public Guid ContentId { get; set; }
    public Guid GenreId { get; set; }

    // ── Navigation ───────────────────────────────────────────────────
    public Content Content { get; set; } = null!;
    public Genre Genre { get; set; } = null!;
}
