using NetflixClone.Domain.Common.Primitives;
using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Domain.Entities.Identity;

public class Profile : AuditableEntity
{
    // ── Foreign key ──────────────────────────────────────────────────
    /// <summary>
    /// FK → ApplicationUser.Id.
    /// String because IdentityUser uses string PKs.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    // ── Basic info ───────────────────────────────────────────────────
    public string Name { get; set; } = string.Empty;

    /// <summary>Azure Blob URL for the chosen avatar image. Null = use default.</summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Profile holder's age.
    /// 0 = not set. Automatically enables IsKidsMode when Age is between 1 and 12.
    /// Used to block content whose MaturityRating exceeds the age-appropriate threshold.
    /// </summary>
    public int Age { get; set; } = 0;

    /// <summary>
    /// When true, content is filtered to G / PG / TV-Y / TV-Y7 / TV-G / TV-PG only.
    /// Auto-set when Age < 13. Can also be toggled manually by the account owner.
    /// </summary>
    public bool IsKidsMode { get; set; } = false;

    /// <summary>
    /// BCrypt-hashed 4-digit PIN.
    /// Null = no PIN set on this profile. Optionally required on profile switch.
    /// </summary>
    public string? PinHash { get; set; }

    /// <summary>
    /// ISO 639-1 language code for UI and content language preference.
    /// Default: "en".
    /// </summary>
    public string PreferredLanguage { get; set; } = "en";


    // ── Navigation ───────────────────────────────────────────────────
    public ApplicationUser User { get; set; } = null!;

    /// <summary>Genre / Actor / Director preferences for this profile.</summary>
    public ICollection<ProfilePreference> Preferences { get; set; } = [];

    public ICollection<Engagement.WatchHistory> WatchHistories { get; set; } = [];
    public ICollection<Engagement.Rating> Ratings { get; set; } = [];
    public ICollection<Media.StreamingSession> StreamingSessions { get; set; } = [];
}
