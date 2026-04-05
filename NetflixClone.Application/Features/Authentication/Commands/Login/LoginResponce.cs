namespace NetflixClone.Application.Features.Authentication.Commands.Login
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
