using Microsoft.AspNetCore.Identity;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Infrastructure.Persistence.Seeds;

public static class UserSeeder
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        // 1. SuperAdmin
        await SeedUserAsync(userManager, "admin@netflixclone.dev", "System", "Admin", "Admin@123456!", "SuperAdmin");
        
        // 2. ContentManager
        await SeedUserAsync(userManager, "content@netflixclone.dev", "Content", "Manager", "Content@123456!", "ContentManager");
        
        // 3. Subscribers
        await SeedUserAsync(userManager, "john@netflixclone.dev", "John", "Doe", "User@123456!", "Subscriber");
        await SeedUserAsync(userManager, "jane@netflixclone.dev", "Jane", "Smith", "User@123456!", "Subscriber");
    }

    private static async Task SeedUserAsync(
        UserManager<ApplicationUser> userManager, 
        string email, 
        string firstName, 
        string lastName, 
        string password, 
        string role)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
            return; // already exists

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
