using NetflixClone.Domain.Common.Primitives;
using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Domain.Entities.Engagement;

public class Rating : AuditableEntity
{
    // ── Foreign keys ─────────────────────────────────────────────────
    public Guid ProfileId { get; set; }
    public Guid ContentId { get; set; }

    // ── Data ─────────────────────────────────────────────────────────
    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            if (value < 1 || value > 5)
                throw new ArgumentOutOfRangeException(nameof(Value), "Rating must be between 1 and 5");

            _value = value;
        }
    }

    public string? Review { get; set; }

    public DateTime RatedAt { get; set; } = DateTime.UtcNow;


    // ── Navigation ───────────────────────────────────────────────────
    public Identity.Profile Profile { get; set; } = null!;
    public Catalog.Content Content { get; set; } = null!;

    // ── Behavior ─────────────────────────────────────────────────────
    public void Update(UpdateRatingData data)
    {
        if (data.Value.HasValue) Value = data.Value.Value;
        if (data.Review != null) Review = data.Review;
    }
}

public class UpdateRatingData
{
    public int? Value { get; set; }
    public string? Review { get; set; }
}
