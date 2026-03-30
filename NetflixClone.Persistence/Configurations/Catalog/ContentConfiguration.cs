using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Catalog;

public class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.ToTable("Contents");

        builder.HasKey(c => c.Id);

        // ── Core columns ─────────────────────────────────────────────
        builder.Property(c => c.ContentType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.OriginalTitle)
            .HasMaxLength(256);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.Tagline)
            .HasMaxLength(512);

        builder.Property(c => c.ReleaseYear)
            .IsRequired();

        builder.Property(c => c.OriginalLanguage)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("en");

        builder.Property(c => c.MaturityRating)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10);

        // ── URL columns ───────────────────────────────────────────────
        builder.Property(c => c.VideoUrl)
            .HasMaxLength(2048);

        builder.Property(c => c.TrailerUrl)
            .HasMaxLength(2048);

        builder.Property(c => c.ThumbnailUrl)
            .HasMaxLength(1024);

        builder.Property(c => c.HeroImageUrl)
            .HasMaxLength(1024);

        // AverageRating is now a stored, denormalized column maintained by
        // Application layer handlers (AddRating, DeleteRating).
        builder.Property(c => c.AverageRating)
            .HasDefaultValue(0m)
            .HasPrecision(3, 2);

        builder.Property(c => c.ViewCount)
            .HasDefaultValue(0L);

        builder.Property(c => c.TotalRatings)
            .HasDefaultValue(0);

        // ── Admin audit ───────────────────────────────────────────────

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        // ── Relationships ─────────────────────────────────────────────
        builder.HasMany(c => c.Seasons)
            .WithOne(s => s.Series)
            .HasForeignKey(s => s.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.ContentGenres)
            .WithOne(cg => cg.Content)
            .HasForeignKey(cg => cg.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.ContentPersons)
            .WithOne(cp => cp.Content)
            .HasForeignKey(cp => cp.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.WatchHistories)
            .WithOne(wh => wh.Content)
            .HasForeignKey(wh => wh.ContentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.Ratings)
            .WithOne(r => r.Content)
            .HasForeignKey(r => r.ContentId)
            .OnDelete(DeleteBehavior.Cascade);


        // ── Indexes ───────────────────────────────────────────────────
        builder.HasIndex(c => c.Slug).IsUnique();
        builder.HasIndex(c => c.ContentType);
        builder.HasIndex(c => c.ReleaseYear);
        builder.HasIndex(c => c.MaturityRating);
        builder.HasIndex(c => c.IsAvailable);
        builder.HasIndex(c => c.ViewCount);
    }
}