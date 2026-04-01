using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Domain.Entities.Identity;
using NetflixClone.Infrastructure.Options;
using NetflixClone.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetflixClone.Infrastructure.Services;

public class JwtTokenGeneration : IJwtTokenGeneration
{
    private readonly JwtOptions _options;

    public JwtTokenGeneration(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    public string GenerateJwtToken(ApplicationUser user, List<string> roles)
    {

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim("IsActive", user.IsActive.ToString()),
            new Claim("IsSuspended", user.IsSuspended.ToString())
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
            Encoding.UTF8.GetBytes(_options.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(expiry),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}