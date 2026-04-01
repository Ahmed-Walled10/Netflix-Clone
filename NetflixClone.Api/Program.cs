using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Infrastructure.Services;
using NetflixClone.Infrastructure.Mail;
using NetflixClone.Infrastructure.Persistence.Seeds;
using NetflixClone.Persistence;
using NetflixClone.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ── Persistence (DbContext + Identity) ────────────────────────────────────
builder.Services.AddPersistenceServices(builder.Configuration);

// ── MediatR ────────────────────────────────────────────────────────────────
// Scans the Application assembly for all IRequestHandler implementations.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(NetflixClone.Application.Features.Authentication.Commands.Login.LoginRequestHandler).Assembly));

// ── AutoMapper ─────────────────────────────────────────────────────────────
builder.Services.AddAutoMapper(
    typeof(NetflixClone.Application.Features.Authentication.Commands.Login.LoginRequestHandler).Assembly);

// ── Application Services ───────────────────────────────────────────────────
// The Application Layer doesn't currently register its own services in its own 
// registration method, so these continue to be registered in infrastructure
builder.Services.AddInfrastructureServices(builder.Configuration);

// ── JWT Bearer Authentication ──────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey   = jwtSettings["SecretKey"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwtSettings["Issuer"],
        ValidAudience            = jwtSettings["Audience"],
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew                = TimeSpan.Zero   // no grace period — tokens expire exactly on time
    };
});

// ── Controllers & Swagger ──────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Netflix Clone API", Version = "v1" });

    // Allow sending the JWT token from Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter your JWT token. Example: Bearer eyJhbGci..."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ── Build ──────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Seed ───────────────────────────────────────────────────────────────────
// Runs migrations and seeds essential data (roles, plans, admin, genres, persons).
// Idempotent — safe to run on every restart.
await DatabaseSeeder.SeedAsync(app.Services);

// ── Middleware ─────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
