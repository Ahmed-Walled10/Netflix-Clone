using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Features.Profiles.Commands.SwitchProfile;
using NetflixClone.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Features.Profiles.Commands.LoginToProfile
{
    public class LoginToProfileRequestHandler : IRequestHandler<LoginToProfileRequest, LoginToProfileResponce>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGeneration _jwtTokenGeneration;
        private readonly IProfileRepository _profileRepository;

        public LoginToProfileRequestHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGeneration jwtTokenGeneration,
            IProfileRepository profileRepository)
        {
            _userManager = userManager;
            _jwtTokenGeneration = jwtTokenGeneration;
            _profileRepository = profileRepository;
        }



        public async Task <LoginToProfileResponce> Handle(
            LoginToProfileRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _profileRepository.GetUserWithSubscriptionsAsync(request.UserId);
            if (user is null)
                throw new UnauthorizedAccessException("User account not found.");



            var profile = user.Profiles.FirstOrDefault(p => p.Id == request.ProfileId);
            if (profile is null)
                throw new KeyNotFoundException(
                    $"Profile {request.ProfileId} does not exist or does not belong to this account.");


            //Verify PIN
            if (profile.PinHash is not null)
            {
                if (string.IsNullOrWhiteSpace(request.Pin))
                    throw new UnauthorizedAccessException(
                        "This profile is PIN-protected. Please provide the PIN.");

                bool pinValid = BCrypt.Net.BCrypt.Verify(request.Pin, profile.PinHash);

                if (!pinValid)
                    throw new UnauthorizedAccessException("Incorrect PIN.");
            }

            var roles = (await _userManager.GetRolesAsync(user)).ToList();

            var profileToken = _jwtTokenGeneration.GenerateProfileJwtToken(user, profile, roles);

            return new LoginToProfileResponce
            {
                AccessToken = profileToken
            };


        }
    }
}
