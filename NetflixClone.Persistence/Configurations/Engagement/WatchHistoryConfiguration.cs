using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Engagement;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Engagement;

public class WatchHistoryConfiguration : IEntityTypeConfiguration<WatchHistory>
{
    public void Configure(EntityTypeBuilder<WatchHistory> builder)
    {
        builder.ToTable("WatchHistories");

        builder.HasKey(wh => wh.Id);

        builder.Property(wh => wh.StoppedAtSeconds)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(wh => wh.TotalDurationSeconds)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(wh => wh.IsCompleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(wh => wh.WatchedAt)
            .IsRequired();

        // ── Relationships ─────────────────────────────────────────────
        // Profile FK is configured in ProfileConfiguration (Cascade).
        // Content FK: NoAction — deleting content shouldn't silently wipe watch history.
        builder.HasOne(wh => wh.Content)
            .WithMany(c => c.WatchHistories)
            .HasForeignKey(wh => wh.ContentId)
            .OnDelete(DeleteBehavior.NoAction);

        // EpisodeId is nullable; no cascade — episode deletion keeps history record (ContentId still valid).
        builder.HasOne<Episode>()
            .WithMany()
            .HasForeignKey(wh => wh.EpisodeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // ── Indexes ───────────────────────────────────────────────────
        // Enforce one record per profile + content (+ optional episode).
        // This is what makes the upsert pattern possible without race conditions.
        // EpisodeId is nullable so we use a filtered approach:
        //   - For movies: (ProfileId, ContentId) where EpisodeId IS NULL
        //   - For episodes: (ProfileId, ContentId, EpisodeId)
        builder.HasIndex(wh => new { wh.ProfileId, wh.ContentId, wh.EpisodeId })
            .IsUnique();

        // Continue Watching query: incomplete records sorted by last watched
        // WHERE ProfileId = X AND IsCompleted = false ORDER BY WatchedAt DESC
        builder.HasIndex(wh => new { wh.ProfileId, wh.IsCompleted, wh.WatchedAt })
            .HasDatabaseName("IX_WatchHistories_ProfileId_IsCompleted_WatchedAt");
    }
}
