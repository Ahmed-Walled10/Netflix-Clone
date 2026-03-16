namespace NetflixClone.Application.Common.Helpers;

/// <summary>
/// Shared utility for generating URL-safe slugs across all Catalog features.
/// </summary>
public static class SlugHelper
{
    /// <summary>
    /// Converts a plain string into a URL-safe slug.
    /// Example: "Romantic Comedy" → "romantic-comedy"
    /// </summary>
    public static string GenerateSlug(string value)
        => value.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("'", string.Empty)
                .Replace(":", string.Empty)
                .Replace(".", string.Empty)
                .Replace(",", string.Empty)
                .Replace("!", string.Empty)
                .Replace("?", string.Empty);

    /// <summary>
    /// Converts a title + release year into a URL-safe slug.
    /// Example: "The Dark Knight", 2008 → "the-dark-knight-2008"
    /// </summary>
    public static string GenerateSlug(string title, int releaseYear)
        => $"{GenerateSlug(title)}-{releaseYear}";
}
