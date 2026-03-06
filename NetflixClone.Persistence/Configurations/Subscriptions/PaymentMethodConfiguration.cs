using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Subscriptions;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");

        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.StripePaymentMethodId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(pm => pm.Brand)
            .IsRequired()
            .HasMaxLength(50); // "visa", "mastercard", "amex"

        builder.Property(pm => pm.Last4)
            .IsRequired()
            .HasMaxLength(4)
            .IsFixedLength(); // CHAR(4) — always exactly 4 digits

        builder.Property(pm => pm.ExpiryMonth)
            .IsRequired();

        builder.Property(pm => pm.ExpiryYear)
            .IsRequired();

        builder.Property(pm => pm.CreatedAt)
            .IsRequired();

        // ── Relationships ────────────────────────────────────────────
        builder.HasOne(pm => pm.User)
            .WithMany(u => u.PaymentMethods)
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Indexes ──────────────────────────────────────────────────
        builder.HasIndex(pm => pm.StripePaymentMethodId)
            .IsUnique();

        // Enforce one default payment method per user
        builder.HasIndex(pm => new { pm.UserId, pm.IsDefault })
            .IsUnique()
            .HasFilter("[IsDefault] = 1");
    }
}
