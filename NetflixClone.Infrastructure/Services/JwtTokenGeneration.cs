using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetflixClone.Application.Contracts;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetflixClone.Application.Services;

public class JwtTokenGeneration : IJwtTokenGeneration
{
    private readonly IConfiguration _configuration;

    public JwtTokenGeneration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateJwtToken(ApplicationUser user, List<string> roles)
    {
        var activePlan = user.Subscriptions
       .FirstOrDefault(s => s.Status == SubscriptionStatus.Active)?
       .Plan;
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim("IsActive", user.IsActive.ToString()),
            new Claim("IsSuspended", user.IsSuspended.ToString()),
            new Claim("Subscriptions", user.Subscriptions.Any(s => s.Status == SubscriptionStatus.Active).ToString()),
            new Claim("MaxProfiles", activePlan?.MaxProfiles.ToString() ?? "0")
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return BuildToken(claims, TimeSpan.FromDays(1));
    }

    public string GenerateProfileJwtToken(ApplicationUser user, Profile profile, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("profileId", profile.Id.ToString()),
            new Claim(ClaimTypes.Name, profile.Name),
            new Claim("avatarUrl", profile.AvatarUrl ?? string.Empty),
            new Claim("isKidsMode", profile.IsKidsMode.ToString().ToLower()),
            new Claim("age", profile.Age.ToString()),
            new Claim("preferredLanguage", profile.PreferredLanguage)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return BuildToken(claims, TimeSpan.FromMinutes(30));
    }

    private string BuildToken(List<Claim> claims, TimeSpan expiry)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.Add(expiry),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}