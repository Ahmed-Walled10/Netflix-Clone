using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Engagement;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Engagement;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("Ratings");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Value)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        // ── Indexes ───────────────────────────────────────────────────
        // One rating per profile per content — the upsert checks this before inserting
        builder.HasIndex(r => new { r.ProfileId, r.ContentId })
            .IsUnique();
    }
}
