using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Authentication.Commands.Logout
{
    public class LogoutRequestHandler : IRequestHandler<LogoutRequest, bool>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LogoutRequestHandler(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<bool> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
