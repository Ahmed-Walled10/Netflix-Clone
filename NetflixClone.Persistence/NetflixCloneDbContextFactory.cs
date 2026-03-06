using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NetflixClone.Infrastructure.Persistence;

/// <summary>
/// Design-time factory used exclusively by EF Core tooling (Add-Migration, etc.).
/// The connection string here is only used at design-time by the tooling;
/// at runtime the app reads it from appsettings.json via Program.cs.
/// </summary>
public sealed class NetflixCloneDbContextFactory
    : IDesignTimeDbContextFactory<NetflixCloneDbContext>
{
    // Design-time only — matches appsettings.json "Default" connection string.
    private const string DesignTimeConnectionString =
        "Server=.;Database=NetflixCloneDb;Trusted_Connection=True;TrustServerCertificate=True";

    public NetflixCloneDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NetflixCloneDbContext>();
        optionsBuilder.UseSqlServer(DesignTimeConnectionString);

        return new NetflixCloneDbContext(optionsBuilder.Options);
    }
}
