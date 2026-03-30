using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Authentication.Commands.Logout
{
    public class LogoutRequestHandler : IRequestHandler<LogoutRequest, bool>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutRequestHandler> _logger;

        public LogoutRequestHandler(SignInManager<ApplicationUser> signInManager, ILogger<LogoutRequestHandler> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }
        public async Task<bool> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed.");
                return false;
            }

        }
    }
}
