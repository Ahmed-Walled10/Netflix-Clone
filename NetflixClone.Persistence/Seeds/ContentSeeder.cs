using Microsoft.EntityFrameworkCore;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Infrastructure.Persistence.Seeds;

public static class ContentSeeder
{
    private const string PlaceholderVideo = ""; // Empty until user uploads
    private const string PlaceholderHero = "https://placehold.co/1920x1080/1a1a1a/e50914?text=Hero+Image";
    private const string PlaceholderThumb = "https://placehold.co/300x450/1a1a1a/e50914?text=Poster";

    // Reusing IDs from GenreSeeder
    private static readonly Guid Action = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid SciFi = Guid.Parse("10000000-0000-0000-0000-000000000012");
    private static readonly Guid Thriller = Guid.Parse("10000000-0000-0000-0000-000000000013");
    private static readonly Guid Crime = Guid.Parse("10000000-0000-0000-0000-000000000005");
    private static readonly Guid Drama = Guid.Parse("10000000-0000-0000-0000-000000000007");

    public static async Task SeedAsync(NetflixCloneDbContext db)
    {
        if (await db.Contents.AnyAsync())
            return; // Already populated

        // 1. Inception (Movie)
        var inceptionId = Guid.NewGuid();
        var inception = new Content
        {
            Id = inceptionId,
            ContentType = ContentType.Movie,
            Title = "Inception",
            Slug = "inception",
            Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
            ReleaseYear = 2010,
            DurationMinutes = 148,
            MaturityRating = MaturityRating.PG13,
            OriginalLanguage = "en",
            IsAvailable = true,
            IsOriginal = false,
            HeroImageUrl = PlaceholderHero,
            ThumbnailUrl = PlaceholderThumb,
            TrailerUrl = PlaceholderVideo,
            VideoUrl = PlaceholderVideo,
            CloudinaryPublicId = null,
            CreatedAt = DateTime.UtcNow
        };

        var incGenres = new List<ContentGenre>
        {
            new() { ContentId = inceptionId, GenreId = Action },
            new() { ContentId = inceptionId, GenreId = SciFi },
            new() { ContentId = inceptionId, GenreId = Thriller },
        };

        var incCast = new List<ContentPerson>
        {
            new() { ContentId = inceptionId, PersonId = PersonSeeder.ChristopherNolan, Role = PersonRole.Director },
            new() { ContentId = inceptionId, PersonId = PersonSeeder.LeonardoDiCaprio, Role = PersonRole.Actor, CharacterName = "Cobb" },
            new() { ContentId = inceptionId, PersonId = PersonSeeder.CillianMurphy, Role = PersonRole.Actor, CharacterName = "Robert Fischer" }
        };


        // 2. Peaky Blinders (TV Show)
        var peakyId = Guid.NewGuid();
        var peaky = new Content
        {
            Id = peakyId,
            ContentType = ContentType.Series,
            Title = "Peaky Blinders",
            Slug = "peaky-blinders",
            Description = "A gangster family epic set in 1900s England, centering on a gang who sew razor blades in the peaks of their caps, and their fierce boss Tommy Shelby.",
            ReleaseYear = 2013,
            EndYear = 2022,
            MaturityRating = MaturityRating.NC17, // Mature
            OriginalLanguage = "en",
            IsAvailable = true,
            IsOriginal = true,
            HeroImageUrl = PlaceholderHero,
            ThumbnailUrl = PlaceholderThumb,
            TrailerUrl = PlaceholderVideo,
            CloudinaryPublicId = null,
            CreatedAt = DateTime.UtcNow
        };

        var peakyGenres = new List<ContentGenre>
        {
            new() { ContentId = peakyId, GenreId = Crime },
            new() { ContentId = peakyId, GenreId = Drama },
        };

        var peakySeason1Id = Guid.NewGuid();
        var peakyCast = new List<ContentPerson>
        {
            new() { ContentId = peakyId, PersonId = PersonSeeder.CillianMurphy, Role = PersonRole.Actor, CharacterName = "Thomas Shelby" }
        };

        var peakySeason1 = new Season
        {
            Id = peakySeason1Id,
            SeriesId = peakyId,
            SeasonNumber = 1,
            Title = "Season 1",
            Description = "Thomas Shelby and his family run the most feared and powerful local gang, the Peaky Blinders.",
            ThumbnailUrl = PlaceholderThumb,
            Episodes = new List<Episode>
            {
                new() { Id = Guid.NewGuid(), SeasonId = peakySeason1Id, EpisodeNumber = 1, Title = "Episode 1", Description = "Tommy gets involved with a shipment of stolen guns.", DurationMinutes = 57, IsAvailable = true, ThumbnailUrl = PlaceholderThumb, VideoUrl = PlaceholderVideo },
                new() { Id = Guid.NewGuid(), SeasonId = peakySeason1Id, EpisodeNumber = 2, Title = "Episode 2", Description = "Tommy fixes a horse race.", DurationMinutes = 58, IsAvailable = true, ThumbnailUrl = PlaceholderThumb, VideoUrl = PlaceholderVideo },
                new() { Id = Guid.NewGuid(), SeasonId = peakySeason1Id, EpisodeNumber = 3, Title = "Episode 3", Description = "Tommy plans to go to Cheltenham.", DurationMinutes = 55, IsAvailable = true, ThumbnailUrl = PlaceholderThumb, VideoUrl = PlaceholderVideo }
            }
        };

        db.Contents.AddRange(inception, peaky);
        db.ContentGenres.AddRange(incGenres.Concat(peakyGenres));
        db.ContentPersons.AddRange(incCast.Concat(peakyCast));
        db.Seasons.AddRange(peakySeason1);

        await db.SaveChangesAsync();
    }
}
