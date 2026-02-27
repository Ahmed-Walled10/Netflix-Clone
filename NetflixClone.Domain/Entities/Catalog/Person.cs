using NetflixClone.Domain.Common.Primitives;

namespace NetflixClone.Domain.Entities.Catalog;


public class Person : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;

    /// <summary>URL-safe slug: "tom-hanks", "christopher-nolan".</summary>
    public string Slug { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? PhotoUrl { get; set; }

    // ── Navigation ───────────────────────────────────────────────────
    public ICollection<ContentPerson> ContentPersons { get; set; } = [];
}
