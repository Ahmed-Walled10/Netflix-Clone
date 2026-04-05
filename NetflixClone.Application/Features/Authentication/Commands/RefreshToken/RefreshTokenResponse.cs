namespace NetflixClone.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiry { get; set; }
}
