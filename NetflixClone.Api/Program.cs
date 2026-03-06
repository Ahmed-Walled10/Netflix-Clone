using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetflixClone.Domain.Entities.Identity;
using NetflixClone.Infrastructure.Persistence;
using NetflixClone.Infrastructure.Persistence.Seeds;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────
builder.Services.AddDbContext<NetflixCloneDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<NetflixCloneDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

// ── Build ─────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Seed ──────────────────────────────────────────────────────────────────
// Runs migrations and seeds essential data (roles, plans, admin, genres, persons).
// Idempotent — safe to run on every restart.
await DatabaseSeeder.SeedAsync(app.Services);

// ── Middleware ────────────────────────────────────────────────────────────
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
