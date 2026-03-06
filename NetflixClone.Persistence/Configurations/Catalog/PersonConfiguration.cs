using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Catalog;
namespace NetflixClone.Infrastructure.Persistence.Configurations.Catalog;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FullName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(p => p.Bio)
            .HasMaxLength(2000);

        builder.Property(p => p.PhotoUrl)
            .HasMaxLength(1024);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasIndex(p => p.Slug).IsUnique();

        // ── Full-Text Search ──────────────────────────────────────────
        // FTS index on FullName is created via raw SQL migration.
        // This enables cast search: searching "Tom Hanks" returns their content.
        // See: Migrations/SqlScripts/FullTextSearch.sql
    }
}

public class ContentPersonConfiguration : IEntityTypeConfiguration<ContentPerson>
{
    public void Configure(EntityTypeBuilder<ContentPerson> builder)
    {
        builder.ToTable("ContentPersons");

        // Composite PK: a person can be both Actor AND Director on the same content
        // but cannot hold the same role twice
        builder.HasKey(cp => new { cp.ContentId, cp.PersonId, cp.Role });

        builder.Property(cp => cp.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(cp => cp.CharacterName)
            .HasMaxLength(256);

        builder.HasOne(cp => cp.Content)
            .WithMany(c => c.ContentPersons)
            .HasForeignKey(cp => cp.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cp => cp.Person)
            .WithMany(p => p.ContentPersons)
            .HasForeignKey(cp => cp.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for queries like "give me all content featuring this actor"
        builder.HasIndex(cp => new { cp.PersonId, cp.Role });
    }
}
