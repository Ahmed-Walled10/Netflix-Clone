using NetflixClone.Domain.Common.Primitives;

namespace NetflixClone.Domain.Entities.Catalog;

public class Season : AuditableEntity
{
    // ── Foreign key ──────────────────────────────────────────────────
    public Guid SeriesId { get; set; }

    // ── Data ─────────────────────────────────────────────────────────
    public int SeasonNumber { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? ReleaseYear { get; set; }

    public string? ThumbnailUrl { get; set; }

    // ── Navigation ───────────────────────────────────────────────────
    public Content Series { get; set; } = null!;
    public ICollection<Episode> Episodes { get; set; } = [];
}
