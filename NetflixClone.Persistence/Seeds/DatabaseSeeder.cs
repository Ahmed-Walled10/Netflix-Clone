using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Identity;
using NetflixClone.Domain.Entities.Subscriptions;


namespace NetflixClone.Infrastructure.Persistence.Seeds;

/// <summary>
/// Seeds essential data that must exist before the app can be used:
///   1. Plans        — the 6 subscription tiers (3 tiers × 2 billing periods)
///   2. Roles        — SuperAdmin, ContentManager, Subscriber
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
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Apply any pending migrations automatically on startup
        await db.Database.MigrateAsync();

        await SeedRolesAsync(roleManager);
        await SeedPlansAsync(db);
        await SeedAdminUserAsync(userManager);
        await GenreSeeder.SeedAsync(db);
        await PersonSeeder.SeedAsync(db);
    }

    // ── Roles ─────────────────────────────────────────────────────────
    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["SuperAdmin", "ContentManager", "Subscriber"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // ── Plans ─────────────────────────────────────────────────────────
    private static async Task SeedPlansAsync(NetflixCloneDbContext db)
    {
        if (await db.Plans.AnyAsync())
            return; // already seeded

        var plans = new List<Plan>
        {
            // ── Basic ────────────────────────────────────────────────
            new()
            {
                Id             = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name           = "basic_monthly",
                DisplayName    = "Basic — Monthly",
                Price          = 8.99m,
                BillingPeriod  = BillingPeriod.Monthly,
                MaxProfiles    = 1,
                MaxVideoQuality = VideoQuality.HD_720p,
                IsActive       = true,
                CreatedAt      = DateTime.UtcNow
            },
            new()
            {
                Id             = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name           = "basic_yearly",
                DisplayName    = "Basic — Yearly",
                Price          = 89.99m,
                BillingPeriod  = BillingPeriod.Yearly,
                MaxProfiles    = 1,
                MaxVideoQuality = VideoQuality.HD_720p,
                IsActive       = true,
                CreatedAt      = DateTime.UtcNow
            },

            // ── Standard ─────────────────────────────────────────────
            new()
            {
                Id             = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name           = "standard_monthly",
                DisplayName    = "Standard — Monthly",
                Price          = 13.99m,
                BillingPeriod  = BillingPeriod.Monthly,
                MaxProfiles    = 3,
                MaxVideoQuality = VideoQuality.FullHD_1080p,
                IsActive       = true,
                CreatedAt      = DateTime.UtcNow
            },
            new()
            {
                Id             = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Name           = "standard_yearly",
                DisplayName    = "Standard — Yearly",
                Price          = 139.99m,
                BillingPeriod  = BillingPeriod.Yearly,
                MaxProfiles    = 3,
                MaxVideoQuality = VideoQuality.FullHD_1080p,
                IsActive       = true,
                CreatedAt      = DateTime.UtcNow
            },

            // ── Premium ───────────────────────────────────────────────
            new()
            {
                Id             = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                Name           = "premium_monthly",
                DisplayName    = "Premium — Monthly",
                Price          = 17.99m,
                BillingPeriod  = BillingPeriod.Monthly,
                MaxProfiles    = 5,
                MaxVideoQuality = VideoQuality.UHD_4K,
                IsActive       = true,
                CreatedAt      = DateTime.UtcNow
            },
            new()
            {
                Id             = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                Name           = "premium_yearly",
                DisplayName    = "Premium — Yearly",
                Price          = 179.99m,
                BillingPeriod  = BillingPeriod.Yearly,
                MaxProfiles    = 5,
                MaxVideoQuality = VideoQuality.UHD_4K,
                IsActive       = true,
                CreatedAt      = DateTime.UtcNow
            },
        };

        await db.Plans.AddRangeAsync(plans);
        await db.SaveChangesAsync();
    }

    // ── Admin user ────────────────────────────────────────────────────
    private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@netflixclone.dev";

        if (await userManager.FindByEmailAsync(adminEmail) is not null)
            return; // already exists

        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,   // admin doesn't need email verification
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // ⚠️ Change this password immediately after first login.
        // In production, load from environment variable / secrets — never hardcode.
        var result = await userManager.CreateAsync(admin, "Admin@123456!");

        if (result.Succeeded)
            await userManager.AddToRoleAsync(admin, "SuperAdmin");
    }

}
