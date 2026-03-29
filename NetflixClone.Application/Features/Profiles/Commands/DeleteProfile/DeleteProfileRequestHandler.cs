using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Application.Persistence;
using System.Security.Claims;
using ProfileEntity = NetflixClone.Domain.Entities.Identity.Profile;

namespace NetflixClone.Application.Features.Profiles.Commands.DeleteProfile;

public class DeleteProfileRequestHandler : IRequestHandler<DeleteProfileRequest, Unit>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IBaseRepository<ProfileEntity> _profileBaseRepository;

    public DeleteProfileRequestHandler(
        IProfileRepository profileRepository,
        IBaseRepository<ProfileEntity> profileBaseRepository)
    {
        _profileRepository = profileRepository;
        _profileBaseRepository = profileBaseRepository;
    }

    public async Task<Unit> Handle(DeleteProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await _profileRepository.GetUserWithProfilesAsync(ClaimTypes.NameIdentifier);
        if (user is null)
            throw new UnauthorizedAccessException("User account not found.");

        var profile = user.Profiles.FirstOrDefault(p => p.Id == request.ProfileId);
        if (profile is null)
            throw new KeyNotFoundException(
                $"Profile {request.ProfileId} does not exist or does not belong to this account.");

        await _profileBaseRepository.DeleteAsync(profile, cancellationToken);
        await _profileBaseRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
