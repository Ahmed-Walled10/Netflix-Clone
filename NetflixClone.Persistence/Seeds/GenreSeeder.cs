using Microsoft.EntityFrameworkCore;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Infrastructure.Persistence.Seeds;

/// <summary>
/// Seeds the standard genre list that must exist before any content can be catalogued.
/// Each genre has a stable, hard-coded Guid so it can be referenced safely in seeding scripts
/// and integration tests without querying the database first.
///
/// Called from DatabaseSeeder.SeedAsync — runs only if no genres exist (idempotent).
/// </summary>
public static class GenreSeeder
{
    public static async Task SeedAsync(NetflixCloneDbContext db)
    {
        if (await db.Genres.AnyAsync())
            return; // already seeded

        var genres = new List<Genre>
        {
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Action",           Slug = "action" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "Adventure",        Slug = "adventure" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Animation",        Slug = "animation" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "Comedy",           Slug = "comedy" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Crime",            Slug = "crime" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Name = "Documentary",      Slug = "documentary" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Name = "Drama",            Slug = "drama" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Name = "Fantasy",          Slug = "fantasy" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000009"), Name = "Horror",           Slug = "horror" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000010"), Name = "Mystery",          Slug = "mystery" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000011"), Name = "Romance",          Slug = "romance" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000012"), Name = "Sci-Fi",           Slug = "sci-fi" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000013"), Name = "Thriller",         Slug = "thriller" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000014"), Name = "Western",          Slug = "western" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000015"), Name = "Biography",        Slug = "biography" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000016"), Name = "History",          Slug = "history" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000017"), Name = "Sport",            Slug = "sport" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000018"), Name = "Music",            Slug = "music" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000019"), Name = "Family",           Slug = "family" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000020"), Name = "Kids",             Slug = "kids" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000021"), Name = "Reality",          Slug = "reality" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000022"), Name = "Stand-Up Comedy",  Slug = "stand-up-comedy" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000023"), Name = "Talk Show",        Slug = "talk-show" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000024"), Name = "True Crime",       Slug = "true-crime" },
            new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000025"), Name = "International",    Slug = "international" },
        };

        await db.Genres.AddRangeAsync(genres);
        await db.SaveChangesAsync();
    }
}
