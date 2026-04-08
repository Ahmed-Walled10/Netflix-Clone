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

    public ICollection<ContentPerson> ContentPersons { get; set; } = [];

    // ── Behavior ─────────────────────────────────────────────────────
    public void Update(UpdatePersonData data)
    {
        if (data.FullName != null) FullName = data.FullName;
        if (data.Slug != null) Slug = data.Slug;
        if (data.Bio != null) Bio = data.Bio;
        if (data.BirthDate.HasValue) BirthDate = data.BirthDate.Value;
        if (data.PhotoUrl != null) PhotoUrl = data.PhotoUrl;
    }
}

public class UpdatePersonData
{
    public string? FullName { get; set; }
    public string? Slug { get; set; }
    public string? Bio { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? PhotoUrl { get; set; }
}
