using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Identity;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.AvatarUrl)
            .HasMaxLength(1024);

        builder.Property(p => p.PinHash)
            .HasMaxLength(512); // BCrypt hash output length

        builder.Property(p => p.PreferredLanguage)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("en");

        builder.Property(p => p.Age)
            .IsRequired()
            .HasDefaultValue(0);

        // ── Relationships ────────────────────────────────────────────
        builder.HasOne(p => p.User)
            .WithMany(u => u.Profiles)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade); // delete user → delete all their profiles

        builder.HasMany(p => p.Preferences)
            .WithOne(pp => pp.Profile)
            .HasForeignKey(pp => pp.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.WatchHistories)
            .WithOne(wh => wh.Profile)
            .HasForeignKey(wh => wh.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Ratings)
            .WithOne(r => r.Profile)
            .HasForeignKey(r => r.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.StreamingSessions)
            .WithOne(ss => ss.Profile)
            .HasForeignKey(ss => ss.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Indexes ──────────────────────────────────────────────────
        // Fast lookup of all profiles for a user
        builder.HasIndex(p => p.UserId);

    }
}
