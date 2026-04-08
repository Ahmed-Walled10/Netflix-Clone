using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using ProfileEntity = NetflixClone.Domain.Entities.Identity.Profile;

namespace NetflixClone.Application.Features.Profiles.Commands.UpdateProfile
{
    public class UpdateProfileRequestHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IBaseRepository<ProfileEntity> _profileBaseRepository;

        public UpdateProfileRequestHandler(
            IProfileRepository profileRepository,
            IBaseRepository<ProfileEntity> profileBaseRepository)
        {
            _profileRepository = profileRepository;
            _profileBaseRepository = profileBaseRepository;
        }

        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _profileRepository.GetUserWithSubscriptionsAndProfilesAsync(request.UserId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            var profileToUpdate = user.Profiles.FirstOrDefault(p => p.Id == request.ProfileId)
                ?? throw new InvalidOperationException("Profile not found or does not belong to the user.");

            string? hashedPin = null;
            if (!string.IsNullOrWhiteSpace(request.Data.PinHash))
            {
                hashedPin = BCrypt.Net.BCrypt.HashPassword(request.Data.PinHash);
                request.Data.PinHash = hashedPin;
            }

            profileToUpdate.Update(request.Data);

            await _profileBaseRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
