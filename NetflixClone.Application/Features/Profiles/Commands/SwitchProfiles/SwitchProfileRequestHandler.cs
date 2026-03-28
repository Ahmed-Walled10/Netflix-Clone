using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Application.Contracts;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Entities.Identity;
using System.Security.Claims;

namespace NetflixClone.Application.Features.Profiles.Commands.SwitchProfile;

public class SwitchProfileRequestHandler : IRequestHandler<SwitchProfileRequest, SwitchProfileResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGeneration _jwtTokenGeneration;
    private readonly IProfileRepository _profileRepository;

    public SwitchProfileRequestHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGeneration jwtTokenGeneration,
        IProfileRepository profileRepository)
    {
        _userManager = userManager;
        _jwtTokenGeneration = jwtTokenGeneration;
        _profileRepository = profileRepository;
    }

    public async Task<SwitchProfileResponse> Handle(
        SwitchProfileRequest request,
        CancellationToken cancellationToken)
    {

        var user = await _profileRepository.GetUserWithSubscriptionsAsync(ClaimTypes.NameIdentifier);
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

        return new SwitchProfileResponse
        {
            AccessToken = profileToken
        };
    }
}