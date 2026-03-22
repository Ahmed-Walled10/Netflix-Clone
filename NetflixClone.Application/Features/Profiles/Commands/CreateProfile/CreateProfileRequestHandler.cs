using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Application.Contracts;
using NetflixClone.Domain.Entities.Identity;
using System.Security.Claims;

namespace NetflixClone.Application.Features.Profiles.Commands.CreateProfile
{
    public class CreateProfileRequestHandler : IRequestHandler<CreateProfileRequest, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGeneration _jwtTokenGeneration;
        public CreateProfileRequestHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGeneration jwtTokenGeneration) 
        {
            _userManager = userManager;
            _jwtTokenGeneration = jwtTokenGeneration;
        }

        public async Task<bool> Handle(CreateProfileRequest request, CancellationToken cancellationToken)
        {
            
        }
    }
}
