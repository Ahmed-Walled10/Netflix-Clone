using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Authentication.Commands.Login
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGeneration _jwtTokenGeneration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginRequestHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenGeneration jwtTokenGeneration,
            IRefreshTokenRepository refreshTokenRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGeneration = jwtTokenGeneration;
            _refreshTokenRepository = refreshTokenRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("No user found");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception("Email not confirmed.");
            }

            if (!user.IsActive)
            {
                throw new Exception("Account has been deactivated.");
            }

            if (user.IsSuspended)
            {
                throw new Exception($"Account is suspended: {user.SuspensionReason}");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                throw new Exception("Incorrect email or password");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Generate access token
            var accessToken = _jwtTokenGeneration.GenerateJwtToken(user, roles.ToList());

            // Generate refresh token
            var plainRefreshToken = _jwtTokenGeneration.GenerateRefreshToken();
            var refreshTokenHash = _jwtTokenGeneration.HashToken(plainRefreshToken);

            // Capture device info from the HTTP request
            var httpContext = _httpContextAccessor.HttpContext;
            var deviceInfo = httpContext?.Request.Headers["User-Agent"].ToString();
            var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString();

            var refreshToken = new Domain.Entities.Identity.RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                DeviceInfo = deviceInfo,
                IpAddress = ipAddress,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtTokenGeneration.RefreshTokenExpiryDays),
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            // Update last login timestamp
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                Token = accessToken,
                RefreshToken = plainRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15),
                Email = user.Email!,
                FullName = user.FirstName + " " + user.LastName,
                Roles = roles.ToList()
            };
        }
    }
}
