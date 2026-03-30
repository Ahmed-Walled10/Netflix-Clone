using NetflixClone.Domain.Common.Primitives;
using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Domain.Entities.Identity;

public class Profile : AuditableEntity
{
    // ── Foreign key ──────────────────────────────────────────────────

    public string UserId { get; set; } = string.Empty;

    // ── Basic info ───────────────────────────────────────────────────
    public string Name { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }

    private int _age;
    public int Age
    {
        get => _age;
        private set => _age = value;
    }

    public bool IsKidsMode { get; private set; } = false;

    /// <summary>
    /// Sets the profile age and automatically enables IsKidsMode when Age is between 1 and 12.
    /// </summary>
    public void SetAge(int age)
    {
        Age = age;
        IsKidsMode = age is > 0 and < 13;
    }

    public string? PinHash { get; set; }

    public string PreferredLanguage { get; set; } = "en";


    // ── Navigation ───────────────────────────────────────────────────
    public ApplicationUser User { get; set; } = null!;

    /// <summary>Genre / Actor / Director preferences for this profile.</summary>
    public ICollection<ProfilePreference> Preferences { get; set; } = [];

    public ICollection<Engagement.WatchHistory> WatchHistories { get; set; } = [];
    public ICollection<Engagement.Rating> Ratings { get; set; } = [];

}
