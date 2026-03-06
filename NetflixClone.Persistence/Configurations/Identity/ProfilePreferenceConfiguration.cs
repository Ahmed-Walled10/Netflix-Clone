using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Identity;

public class ProfilePreferenceConfiguration : IEntityTypeConfiguration<ProfilePreference>
{
    public void Configure(EntityTypeBuilder<ProfilePreference> builder)
    {
        builder.ToTable("ProfilePreferences");

        builder.HasKey(pp => pp.Id);

        builder.Property(pp => pp.ReferenceId)
            .IsRequired();

        builder.Property(pp => pp.ReferenceName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(pp => pp.CreatedAt)
            .IsRequired();

        // ── Indexes ──────────────────────────────────────────────────
        // Fast retrieval of all preferences for a profile
        builder.HasIndex(pp => pp.ProfileId);

    }
}
