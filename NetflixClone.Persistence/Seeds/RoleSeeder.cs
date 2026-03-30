using Microsoft.AspNetCore.Identity;

namespace NetflixClone.Infrastructure.Persistence.Seeds;

public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["SuperAdmin", "ContentManager", "Subscriber", "NotSubscriber"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
