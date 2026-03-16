using MediatR;
using NetflixClone.Domain.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Content.Commands.CreateContent;

public class CreateContentRequest : IRequest<CreateContentResponse>
{
    // ── Core identity ─────────────────────────────────────────────────────────

    [Required]
    public ContentType ContentType { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(200)]
    public string? OriginalTitle { get; set; }

    
    [StringLength(220)]
    public string? Slug { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Tagline { get; set; }

    [Required]
    [Range(1888, 2100)]
    public int ReleaseYear { get; set; }

    
    [Range(1888, 2100)]
    public int? EndYear { get; set; }

    [Range(1, 1200)]
    public int? DurationMinutes { get; set; }

    // ── Classification ────────────────────────────────────────────────────────

    /// <summary>Age-restriction rating (G, PG, PG-13, TV-14, TV-MA, NC-17 …).</summary>
    [Required]
    public MaturityRating MaturityRating { get; set; }

    
    [StringLength(10)]
    public string OriginalLanguage { get; set; } = "en";

    // ── Media URLs (MVP — direct blob links, replaced by asset pipeline in v2) ─

    /// <summary>
    /// Direct Azure Blob Storage URL for the main MP4 file.
    /// Required for Movie / Documentary. Optional at creation for Series (uploaded per-episode).
    /// </summary>
    [Url]
    public string? VideoUrl { get; set; }

    /// <summary>Trailer video URL. Optional.</summary>
    [Url]
    public string? TrailerUrl { get; set; }

    // ── Images ────────────────────────────────────────────────────────────────

    /// <summary>Portrait/poster image shown in browse grids. Optional at creation.</summary>
    [Url]
    public string? ThumbnailUrl { get; set; }

    /// <summary>Wide landscape hero banner image. Optional at creation.</summary>
    [Url]
    public string? HeroImageUrl { get; set; }

    // ── Flags ─────────────────────────────────────────────────────────────────

    public bool IsAvailable { get; set; } = false;

    public bool IsOriginal { get; set; } = false;

    // ── Genres & people ───────────────────────────────────────────────────────


    public List<Guid> GenreIds { get; set; } = [];

    public List<ContentPersonRequest> Persons { get; set; } = [];

    // ── Series-only: Seasons → Episodes ───────────────────────────────────────

    public List<CreateSeasonRequest> Seasons { get; set; } = [];
}

// ─────────────────────────────────────────────────────────────────────────────
// Nested DTOs
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>A season belonging to a Series, submitted as part of CreateContentRequest.</summary>
public class CreateSeasonRequest
{
    [Required]
    [Range(1, 100)]
    public int SeasonNumber { get; set; }

    [StringLength(200)]
    public string? Title { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Range(1888, 2100)]
    public int? ReleaseYear { get; set; }

    [Url]
    public string? ThumbnailUrl { get; set; }

    public List<CreateEpisodeRequest> Episodes { get; set; } = [];
}

/// <summary>A single episode within a season, submitted as part of CreateSeasonRequest.</summary>
public class CreateEpisodeRequest
{
    [Required]
    [Range(1, 500)]
    public int EpisodeNumber { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Range(1, 600)]
    public int DurationMinutes { get; set; }

    [Range(1888, 2100)]
    public int? ReleaseYear { get; set; }

    public DateOnly? AirDate { get; set; }

    /// <summary>Episode still / thumbnail image URL. Optional.</summary>
    [Url]
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Direct blob URL for this episode's MP4.
    /// Null at creation is acceptable — admin can upload later.
    /// </summary>
    [Url]
    public string? VideoUrl { get; set; }

    public bool IsAvailable { get; set; } = false;
}

/// <summary>Links a person (actor, director, writer …) to the content being created.</summary>
public class ContentPersonRequest
{
    /// <summary>ID of an existing Person row in the database.</summary>
    [Required]
    public Guid PersonId { get; set; }

    /// <summary>
    /// Role this person played in the production.
    /// Uses the PersonRole enum (Actor, Director, Writer, Producer …).
    /// </summary>
    [Required]
    public PersonRole Role { get; set; }

    /// <summary>Character name for actors. Null for crew roles.</summary>
    [StringLength(150)]
    public string? CharacterName { get; set; }

    /// <summary>Order in the credits list. Lower = more prominent (1 = top billing).</summary>
    [Range(1, 1000)]
    public int? DisplayOrder { get; set; }
}

