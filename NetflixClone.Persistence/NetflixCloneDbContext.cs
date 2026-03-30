using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetflixClone.Domain.Entities.Identity;
using NetflixClone.Domain.Entities.Subscriptions;
using NetflixClone.Domain.Entities.Catalog;

using NetflixClone.Domain.Entities.Engagement;
using SubscriptionEntity = NetflixClone.Domain.Entities.Subscriptions.Subscription;


namespace NetflixClone.Infrastructure.Persistence
{
    public class NetflixCloneDbContext : IdentityDbContext<ApplicationUser>
    {
        public NetflixCloneDbContext(DbContextOptions<NetflixCloneDbContext> options)
       : base(options) { }

        // ── Identity ─────────────────────────────────────────────────────
        // ApplicationUser is handled by IdentityDbContext<ApplicationUser>
        public DbSet<Profile> Profiles => Set<Profile>();
        public DbSet<ProfilePreference> ProfilePreferences => Set<ProfilePreference>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        // ── Subscriptions ────────────────────────────────────────────────
        public DbSet<Plan> Plans => Set<Plan>();
        public DbSet<SubscriptionEntity> Subscriptions => Set<SubscriptionEntity>();
        public DbSet<Invoice> Invoices => Set<Invoice>();

        // ── Catalog ──────────────────────────────────────────────────────
        public DbSet<Content> Contents => Set<Content>();
        public DbSet<Season> Seasons => Set<Season>();
        public DbSet<Episode> Episodes => Set<Episode>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<ContentGenre> ContentGenres => Set<ContentGenre>();
        public DbSet<Person> Persons => Set<Person>();
        public DbSet<ContentPerson> ContentPersons => Set<ContentPerson>();


        // ── Engagement ───────────────────────────────────────────────────
        public DbSet<WatchHistory> WatchHistories => Set<WatchHistory>();
        public DbSet<Rating> Ratings => Set<Rating>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // MUST be called first — configures all Identity tables
            base.OnModelCreating(builder);

            // Apply every IEntityTypeConfiguration in this assembly automatically.
            // Any new configuration class added to Configurations/ is picked up
            // without touching this file.
            builder.ApplyConfigurationsFromAssembly(typeof(NetflixCloneDbContext).Assembly);
        }

    }
}
