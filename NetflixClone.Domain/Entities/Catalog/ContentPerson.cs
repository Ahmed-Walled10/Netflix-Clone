using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Domain.Entities.Catalog;

public class ContentPerson
{
    public Guid ContentId { get; set; }
    public Guid PersonId { get; set; }
    public PersonRole Role { get; set; }

    /// <summary>For actors: the character name they portray. Null for crew roles.</summary>
    public string? CharacterName { get; set; }

    // ── Navigation ───────────────────────────────────────────────────
    public Content Content { get; set; } = null!;
    public Person Person { get; set; } = null!;
}
