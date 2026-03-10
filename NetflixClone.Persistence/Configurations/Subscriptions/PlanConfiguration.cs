using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Subscriptions;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("Plans");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.DisplayName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(10, 2); // e.g. 13.99

        builder.Property(p => p.BillingPeriod)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.MaxVideoQuality)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.StripePriceId)
            .HasMaxLength(256);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // ── Indexes ──────────────────────────────────────────────────
        // Plan names must be unique — prevents accidental duplicates during seeding
        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}
