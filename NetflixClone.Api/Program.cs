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
using NetflixClone.Application;

var builder = WebApplication.CreateBuilder(args);

// ── Persistence (DbContext + Identity) ────────────────────────────────────
builder.Services.AddPersistenceServices(builder.Configuration);



// ── AutoMapper ─────────────────────────────────────────────────────────────
// AutoMapper v13+ ships AddAutoMapper() in the core package (DI extension pkg removed).
// v16 changed: every overload requires Action<IMapperConfigurationExpression> as first arg.
builder.Services.AddAutoMapper(
    cfg => { },   // no extra configuration needed — profiles are scanned from the assembly
    typeof(NetflixClone.Application.Features.Authentication.Commands.Login.LoginRequestHandler));

//-- CORS ───────────────────────────────────────────────────────────────
// ── CORS ── (ONE policy only, replaces both old AddCors calls)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                // "https://netflix-clone-deployed-theta.vercel.app", // Deployment
                "http://localhost:5173"  // Local dev
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
//--------------------------

// ── Application Services ───────────────────────────────────────────────────
builder.Services.AddApplicationServices();
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

await DatabaseSeeder.SeedAsync(app.Services);

// Swagger
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

// ✅ Must be FIRST before everything
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/subscription/webhook"))
        context.Request.EnableBuffering();
    await next();
});

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();