using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Media;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Media;

public class StreamingSessionConfiguration : IEntityTypeConfiguration<StreamingSession>
{
    public void Configure(EntityTypeBuilder<StreamingSession> builder)
    {
        builder.ToTable("StreamingSessions");

        builder.HasKey(ss => ss.Id);

        builder.Property(ss => ss.DeviceType)
            .HasMaxLength(100);

        builder.Property(ss => ss.DeviceId)
            .HasMaxLength(256);

        builder.Property(ss => ss.IpAddress)
            .HasMaxLength(45); // IPv6 max length

        builder.Property(ss => ss.StartedAt)
            .IsRequired();

        builder.Property(ss => ss.LastHeartbeatAt)
            .IsRequired();

        // ── Relationships ─────────────────────────────────────────────
        // Profile → Sessions: cascade (delete profile = delete its sessions)
        builder.HasOne(ss => ss.Profile)
            .WithMany(p => p.StreamingSessions)
            .HasForeignKey(ss => ss.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Content → Sessions: NoAction (keep session logs even if content is removed)
        builder.HasOne(ss => ss.Content)
            .WithMany()
            .HasForeignKey(ss => ss.ContentId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── Indexes ───────────────────────────────────────────────────
        // THE most critical index in the entire app.
        // The concurrent stream check runs on EVERY stream start:
        //   WHERE ProfileId = X AND IsActive = true
        //   AND LastHeartbeatAt > NOW() - 2 minutes
        // This index must cover all three conditions.
        builder.HasIndex(ss => new { ss.ProfileId, ss.IsActive, ss.LastHeartbeatAt })
            .HasDatabaseName("IX_StreamingSessions_ProfileId_IsActive_Heartbeat");

        // Find sessions by content (e.g. admin analytics)
        builder.HasIndex(ss => ss.ContentId);
    }
}
