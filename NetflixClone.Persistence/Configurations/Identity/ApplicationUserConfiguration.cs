using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Identity;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Identity already sets the table name to "AspNetUsers".
        // We only configure the extra columns we added.

        builder.Property(u => u.StripeCustomerId)
            .HasMaxLength(256);

        builder.Property(u => u.SuspensionReason)
            .HasMaxLength(512);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired();

        // Index on StripeCustomerId for fast webhook lookups
        // (Stripe sends cus_xxxx and we need to find the user immediately)
        builder.HasIndex(u => u.StripeCustomerId)
            .IsUnique()
            .HasFilter("[StripeCustomerId] IS NOT NULL"); // partial index — null users are excluded
    }
}
