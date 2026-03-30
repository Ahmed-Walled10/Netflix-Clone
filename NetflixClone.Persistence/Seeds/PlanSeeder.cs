using Microsoft.EntityFrameworkCore;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Infrastructure.Persistence.Seeds;

public static class PlanSeeder
{
    public static async Task SeedAsync(NetflixCloneDbContext db)
    {
        if (await db.Plans.AnyAsync())
            return; // already seeded

        var plans = new List<Plan>
        {
            // ── Basic ────────────────────────────────────────────────
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "basic_monthly", DisplayName = "Basic — Monthly", Price = 8.99m, BillingPeriod = BillingPeriod.Monthly, MaxProfiles = 1, MaxVideoQuality = VideoQuality.HD_720p, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "basic_yearly", DisplayName = "Basic — Yearly", Price = 89.99m, BillingPeriod = BillingPeriod.Yearly, MaxProfiles = 1, MaxVideoQuality = VideoQuality.HD_720p, IsActive = true, CreatedAt = DateTime.UtcNow },

            // ── Standard ─────────────────────────────────────────────
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "standard_monthly", DisplayName = "Standard — Monthly", Price = 13.99m, BillingPeriod = BillingPeriod.Monthly, MaxProfiles = 3, MaxVideoQuality = VideoQuality.FullHD_1080p, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "standard_yearly", DisplayName = "Standard — Yearly", Price = 139.99m, BillingPeriod = BillingPeriod.Yearly, MaxProfiles = 3, MaxVideoQuality = VideoQuality.FullHD_1080p, IsActive = true, CreatedAt = DateTime.UtcNow },

            // ── Premium ───────────────────────────────────────────────
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Name = "premium_monthly", DisplayName = "Premium — Monthly", Price = 17.99m, BillingPeriod = BillingPeriod.Monthly, MaxProfiles = 5, MaxVideoQuality = VideoQuality.UHD_4K, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Name = "premium_yearly", DisplayName = "Premium — Yearly", Price = 179.99m, BillingPeriod = BillingPeriod.Yearly, MaxProfiles = 5, MaxVideoQuality = VideoQuality.UHD_4K, IsActive = true, CreatedAt = DateTime.UtcNow },
        };

        await db.Plans.AddRangeAsync(plans);
        await db.SaveChangesAsync();
    }
}
