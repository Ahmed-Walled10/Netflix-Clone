using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Infrastructure.Persistence.Configurations.Catalog;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Slug)
            .IsRequired()
            .HasMaxLength(120);

        builder.HasIndex(g => g.Name).IsUnique();
        builder.HasIndex(g => g.Slug).IsUnique();
    }
}

public class ContentGenreConfiguration : IEntityTypeConfiguration<ContentGenre>
{
    public void Configure(EntityTypeBuilder<ContentGenre> builder)
    {
        builder.ToTable("ContentGenres");

        // Composite primary key — no surrogate Id needed for pure junction tables
        builder.HasKey(cg => new { cg.ContentId, cg.GenreId });

        builder.HasOne(cg => cg.Content)
            .WithMany(c => c.ContentGenres)
            .HasForeignKey(cg => cg.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cg => cg.Genre)
            .WithMany(g => g.ContentGenres)
            .HasForeignKey(cg => cg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
