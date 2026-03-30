using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace NetflixClone.Infrastructure.Persistence.Seeds;

/// <summary>
/// Seeds essential data that must exist before the app can be used:
///   1. Plans        — the 6 subscription tiers (3 tiers × 2 billing periods)
///   2. Roles        — SuperAdmin, ContentManager, Subscriber, NotSubscriber
///   3. Admin user   — one SuperAdmin account for first login
///
/// Called from Program.cs at startup (only runs if data is missing — idempotent).
/// Safe to run on every restart.
///
/// Usage in Program.cs:
///   await DatabaseSeeder.SeedAsync(app.Services);
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NetflixCloneDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<NetflixClone.Domain.Entities.Identity.ApplicationUser>>();

        // Apply any pending migrations automatically on startup
        await Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.MigrateAsync(db.Database);

        // 1. Identity
        await RoleSeeder.SeedAsync(roleManager);
        await UserSeeder.SeedAsync(userManager);

        // 2. Subscriptions
        await PlanSeeder.SeedAsync(db);

        // 3. Catalog Base Data
        await GenreSeeder.SeedAsync(db);
        await PersonSeeder.SeedAsync(db);

        // 4. Catalog Media
        await ContentSeeder.SeedAsync(db);
    }
}
