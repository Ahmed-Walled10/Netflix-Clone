using Microsoft.EntityFrameworkCore;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Infrastructure.Persistence.Seeds;

/// <summary>
/// Seeds a set of well-known actors and directors.
/// These are reference-quality rows used for development, demos, and integration tests.
/// Stable Guids allow ContentPersonSeeder (future) to reference them without DB lookups.
///
/// Called from DatabaseSeeder.SeedAsync — idempotent.
/// </summary>
public static class PersonSeeder
{
    // ── Stable IDs (copy these into ContentPersonSeeder when you seed cast) ──
    public static readonly Guid ChristopherNolan   = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid DavidFincher       = Guid.Parse("20000000-0000-0000-0000-000000000002");
    public static readonly Guid RidleyScott        = Guid.Parse("20000000-0000-0000-0000-000000000003");
    public static readonly Guid MartinScorsese     = Guid.Parse("20000000-0000-0000-0000-000000000004");
    public static readonly Guid StevenSpielberg    = Guid.Parse("20000000-0000-0000-0000-000000000005");

    public static readonly Guid LeonardoDiCaprio   = Guid.Parse("20000000-0000-0000-0000-000000000010");
    public static readonly Guid CillianMurphy      = Guid.Parse("20000000-0000-0000-0000-000000000011");
    public static readonly Guid BradPitt           = Guid.Parse("20000000-0000-0000-0000-000000000012");
    public static readonly Guid TomHanks           = Guid.Parse("20000000-0000-0000-0000-000000000013");
    public static readonly Guid RussellCrowe       = Guid.Parse("20000000-0000-0000-0000-000000000014");
    public static readonly Guid AnthonyHopkins     = Guid.Parse("20000000-0000-0000-0000-000000000015");
    public static readonly Guid KeanuReeves        = Guid.Parse("20000000-0000-0000-0000-000000000016");

    public static readonly Guid ScarJohansson     = Guid.Parse("20000000-0000-0000-0000-000000000020");
    public static readonly Guid NataliePortman     = Guid.Parse("20000000-0000-0000-0000-000000000021");
    public static readonly Guid CateBlanchett      = Guid.Parse("20000000-0000-0000-0000-000000000022");
    public static readonly Guid MerylStreep        = Guid.Parse("20000000-0000-0000-0000-000000000023");

    public static async Task SeedAsync(NetflixCloneDbContext db)
    {
        if (await db.Persons.AnyAsync())
            return;

        var persons = new List<Person>
        {
            // ── Directors ──────────────────────────────────────────────
            new() { Id = ChristopherNolan,   FullName = "Christopher Nolan",   Slug = "christopher-nolan",   Bio = "British-American filmmaker known for mind-bending narratives.",  BirthDate = new DateOnly(1970, 7, 30) },
            new() { Id = DavidFincher,       FullName = "David Fincher",       Slug = "david-fincher",       Bio = "American director known for dark, stylized thrillers.",           BirthDate = new DateOnly(1962, 8, 28) },
            new() { Id = RidleyScott,        FullName = "Ridley Scott",        Slug = "ridley-scott",        Bio = "British director of epic science-fiction and historical films.",  BirthDate = new DateOnly(1937, 11, 30) },
            new() { Id = MartinScorsese,     FullName = "Martin Scorsese",     Slug = "martin-scorsese",     Bio = "American filmmaker known for crime dramas and gangster films.",   BirthDate = new DateOnly(1942, 11, 17) },
            new() { Id = StevenSpielberg,    FullName = "Steven Spielberg",    Slug = "steven-spielberg",    Bio = "American director, producer, and screenwriter.",                 BirthDate = new DateOnly(1946, 12, 18) },

            // ── Actors ─────────────────────────────────────────────────
            new() { Id = LeonardoDiCaprio,   FullName = "Leonardo DiCaprio",   Slug = "leonardo-dicaprio",   Bio = "American actor and film producer.",                              BirthDate = new DateOnly(1974, 11, 11) },
            new() { Id = CillianMurphy,      FullName = "Cillian Murphy",      Slug = "cillian-murphy",      Bio = "Irish actor known for Peaky Blinders and Oppenheimer.",          BirthDate = new DateOnly(1976, 5, 25) },
            new() { Id = BradPitt,           FullName = "Brad Pitt",           Slug = "brad-pitt",           Bio = "American actor and film producer.",                              BirthDate = new DateOnly(1963, 12, 18) },
            new() { Id = TomHanks,           FullName = "Tom Hanks",           Slug = "tom-hanks",           Bio = "American actor and filmmaker.",                                  BirthDate = new DateOnly(1956, 7, 9) },
            new() { Id = RussellCrowe,       FullName = "Russell Crowe",       Slug = "russell-crowe",       Bio = "New Zealand-Australian actor and director.",                     BirthDate = new DateOnly(1964, 4, 7) },
            new() { Id = AnthonyHopkins,     FullName = "Anthony Hopkins",     Slug = "anthony-hopkins",     Bio = "Welsh actor known for The Silence of the Lambs.",               BirthDate = new DateOnly(1937, 12, 31) },
            new() { Id = KeanuReeves,        FullName = "Keanu Reeves",        Slug = "keanu-reeves",        Bio = "Canadian actor known for The Matrix trilogy and John Wick.",    BirthDate = new DateOnly(1964, 9, 2) },

            // ── Actresses ──────────────────────────────────────────────
            new() { Id = ScarJohansson,      FullName = "Scarlett Johansson",  Slug = "scarlett-johansson",  Bio = "American actress known for Black Widow and Lost in Translation.", BirthDate = new DateOnly(1984, 11, 22) },
            new() { Id = NataliePortman,     FullName = "Natalie Portman",     Slug = "natalie-portman",     Bio = "Israeli-American actress and filmmaker.",                        BirthDate = new DateOnly(1981, 6, 9) },
            new() { Id = CateBlanchett,      FullName = "Cate Blanchett",      Slug = "cate-blanchett",      Bio = "Australian actress and theatre director.",                       BirthDate = new DateOnly(1969, 5, 14) },
            new() { Id = MerylStreep,        FullName = "Meryl Streep",        Slug = "meryl-streep",        Bio = "American actress, widely regarded as the greatest of her era.",  BirthDate = new DateOnly(1949, 6, 22) },
        };

        await db.Persons.AddRangeAsync(persons);
        await db.SaveChangesAsync();
    }
}
