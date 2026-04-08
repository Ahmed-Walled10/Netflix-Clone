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
// AutoMapper v13+ ships AddAutoMapper() in the core package (DI extension pkg removed).
// v16 changed: every overload requires Action<IMapperConfigurationExpression> as first arg.
builder.Services.AddAutoMapper(
    cfg => { },   // no extra configuration needed — profiles are scanned from the assembly
    typeof(NetflixClone.Application.Features.Authentication.Commands.Login.LoginRequestHandler));

//-- CORS ───────────────────────────────────────────────────────────────
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
       p.WithOrigins("http://localhost:5173")
       .AllowAnyHeader()
       .AllowAnyMethod()));
//--------------------------

// ── Application Services ───────────────────────────────────────────────────
// The Application Layer doesn't currently register its own services in its own 
// registration method, so these continue to be registered in infrastructure
builder.Services.AddHttpContextAccessor();
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "https://your-app.vercel.app",       // if React is on Vercel
                "https://yoursite.monsterasp.net"    // if React is served from same host
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Logging.AddFilter("LuckyPennySoftware.MediatR.License", LogLevel.None);
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

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/subscription/webhook"))
    {
        context.Request.EnableBuffering();
    }
    await next();
});

//app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();
