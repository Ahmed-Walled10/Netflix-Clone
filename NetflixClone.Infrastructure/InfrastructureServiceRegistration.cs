using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Infrastructure.Mail;
using NetflixClone.Infrastructure.Options;
using NetflixClone.Infrastructure.Services;

namespace NetflixClone.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ── Options Configuration ──────────────────────────────────────────
        services.Configure<StripeOptions>(configuration.GetSection("Stripe"));
        services.Configure<CloudinaryOptions>(configuration.GetSection("Cloudinary"));
        services.Configure<EmailOptions>(configuration.GetSection("EmailSettings"));
        services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));

        // ── Services Registration ──────────────────────────────────────────
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtTokenGeneration, JwtTokenGeneration>();
        services.AddScoped<IOtpService, OtpService>();

        return services;
    }
}
