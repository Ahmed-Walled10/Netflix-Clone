using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Application.Contracts;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Authentication.Commands.Login
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, LoginResponse>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGeneration _jwtTokenGeneration;
        public LoginRequestHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtTokenGeneration jwtTokenGeneration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGeneration = jwtTokenGeneration;
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

            var token = _jwtTokenGeneration.GenerateJwtToken(user, roles.ToList());

            return new LoginResponse
            {
                Token = token,
                Email = user.Email!,
                FullName = user.FirstName + " " + user.LastName,
                Roles = roles.ToList()
            };
        }
    }
}
