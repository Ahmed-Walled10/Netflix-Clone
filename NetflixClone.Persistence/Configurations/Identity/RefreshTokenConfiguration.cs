using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Identity;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.TokenHash)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(rt => rt.DeviceInfo)
            .HasMaxLength(512);

        builder.Property(rt => rt.IpAddress)
            .HasMaxLength(45); // max length for IPv6

        builder.Property(rt => rt.ReplacedByTokenHash)
            .HasMaxLength(512);

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder.Property(rt => rt.CreatedAt)
            .IsRequired();

        // Ignore computed properties — these are C# only, not columns
        builder.Ignore(rt => rt.IsExpired);
        builder.Ignore(rt => rt.IsRevoked);
        builder.Ignore(rt => rt.IsActive);

        // ── Relationships ────────────────────────────────────────────
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade); // delete user → delete their tokens

        // ── Indexes ──────────────────────────────────────────────────
        // The most common query: look up a token by its hash on every /auth/refresh
        builder.HasIndex(rt => rt.TokenHash)
            .IsUnique();

        // Find all tokens for a user (e.g. "log out everywhere")
        builder.HasIndex(rt => rt.UserId);
    }
}
