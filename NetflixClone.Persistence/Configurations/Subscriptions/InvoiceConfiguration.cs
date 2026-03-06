using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Subscriptions;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Subscriptions;
public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Amount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(i => i.Currency)
            .IsRequired()
            .HasMaxLength(3) // ISO 4217: "usd", "eur", etc.
            .HasDefaultValue("usd");

        builder.Property(i => i.PeriodStart)
            .IsRequired();

        builder.Property(i => i.PeriodEnd)
            .IsRequired();

        builder.Property(i => i.PaidAt)
            .IsRequired(); // non-nullable: every invoice row IS a paid invoice

        builder.Property(i => i.StripeInvoiceId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(i => i.StripePdfUrl)
            .HasMaxLength(2048);

        builder.Property(i => i.CreatedAt)
            .IsRequired();

        // ── Indexes ──────────────────────────────────────────────────
        // Webhook idempotency: before inserting, check StripeInvoiceId doesn't already exist
        builder.HasIndex(i => i.StripeInvoiceId)
            .IsUnique();

        // Invoice history page: find all invoices for a subscription ordered by date
        builder.HasIndex(i => new { i.SubscriptionId, i.PaidAt });
    }
}
