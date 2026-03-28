using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Identity;
using NetflixClone.Infrastructure.Persistence;
using NetflixClone.Persistence.Repositories;

namespace NetflixClone.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ── Database ───────────────────────────────────────────────────
            services.AddDbContext<NetflixCloneDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            // ── Identity ───────────────────────────────────────────────────
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit            = false;
                options.Password.RequireLowercase        = false;
                options.Password.RequireUppercase        = false;
                options.Password.RequireNonAlphanumeric  = false;
                options.Password.RequiredLength          = 6;
                options.User.RequireUniqueEmail          = true;
                options.Lockout.DefaultLockoutTimeSpan   = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts  = 5;
                options.Lockout.AllowedForNewUsers       = true;
            })
            .AddEntityFrameworkStores<NetflixCloneDbContext>()
            .AddDefaultTokenProviders();

            // ── Repositories ───────────────────────────────────────────────
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IContentRepository, ContentRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IWatchHistoryRepository, WatchHistoryRepository>();

            return services;
        }
    }
}
