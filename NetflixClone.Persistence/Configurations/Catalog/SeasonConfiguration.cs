using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Catalog;
namespace NetflixClone.Infrastructure.Persistence.Configurations.Catalog;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.ToTable("Seasons");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SeasonNumber)
            .IsRequired();

        builder.Property(s => s.Title)
            .HasMaxLength(256);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.ThumbnailUrl)
            .HasMaxLength(1024);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        // ── Relationships ─────────────────────────────────────────────
        builder.HasMany(s => s.Episodes)
            .WithOne(e => e.Season)
            .HasForeignKey(e => e.SeasonId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Indexes ───────────────────────────────────────────────────
        // Enforce unique season numbers within a series
        builder.HasIndex(s => new { s.SeriesId, s.SeasonNumber })
            .IsUnique();
    }
}
