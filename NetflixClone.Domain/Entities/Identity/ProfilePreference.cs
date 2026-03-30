using NetflixClone.Domain.Common.Primitives;

namespace NetflixClone.Domain.Entities.Identity;

public class ProfilePreference : AuditableEntity
{
    // ── Foreign key ──────────────────────────────────────────────────
    public Guid ProfileId { get; set; }

    /// <summary>
    /// The ID of the Genre or Person this preference points to.
    /// Stored as Guid. No FK constraint — referential integrity checked at app layer
    /// to avoid cross-context FK coupling in the database.
    /// </summary>
    public Guid ReferenceId { get; set; }

    /// <summary>
    /// Denormalized display name (e.g. "Action", "Tom Hanks", "Christopher Nolan").
    /// Avoids a join when rendering the preferences list.
    /// </summary>
    public string ReferenceName { get; set; } = string.Empty;

    // ── Navigation ───────────────────────────────────────────────────
    public Profile Profile { get; set; } = null!;
}
