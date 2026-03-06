using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Catalog;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.ToTable("Episodes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EpisodeNumber)
            .IsRequired();

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.DurationMinutes)
            .IsRequired();

        builder.Property(e => e.ThumbnailUrl)
            .HasMaxLength(1024);

        builder.Property(e => e.VideoUrl)
            .HasMaxLength(2048);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // ── Indexes ───────────────────────────────────────────────────
        // Enforce unique episode numbers within a season
        builder.HasIndex(e => new { e.SeasonId, e.EpisodeNumber })
            .IsUnique();
    }
}
