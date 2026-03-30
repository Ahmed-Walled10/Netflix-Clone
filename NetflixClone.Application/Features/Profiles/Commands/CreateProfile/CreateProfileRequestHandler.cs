using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Domain.Entities.Identity;
using System.Security.Claims;
using ProfileEntity = NetflixClone.Domain.Entities.Identity.Profile;

namespace NetflixClone.Application.Features.Profiles.Commands.CreateProfile
{
    public class CreateProfileRequestHandler : IRequestHandler<CreateProfileRequest, CreateProfileResponce>
    {
        private readonly IBaseRepository<ProfileEntity> _profileBaseRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGeneration _jwtTokenGeneration;
        private readonly IMapper _mapper;
        public CreateProfileRequestHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGeneration jwtTokenGeneration,
            IProfileRepository profileRepository,
            IBaseRepository<ProfileEntity> profileBaseRepository,
            IMapper mapper) 
        {
            _userManager = userManager;
            _jwtTokenGeneration = jwtTokenGeneration;
            _profileRepository = profileRepository;
            _profileBaseRepository = profileBaseRepository;
            _mapper = mapper;
        }

        public async Task<CreateProfileResponce> Handle(CreateProfileRequest request, CancellationToken cancellationToken)
        {

            var user = await _profileRepository.GetUserWithSubscriptionsAsync(request.UserId);
            if (user == null)
                throw new UnauthorizedAccessException("User account not found.");

            var activeSubscription = user.Subscriptions
            .FirstOrDefault(s => s.Status == SubscriptionStatus.Active);
            int maxProfiles;

            if (activeSubscription is null)
                throw new InvalidOperationException(
                    "You need an active subscription to create profiles. " +
                    "Please subscribe to a plan to add profiles.");

            maxProfiles = activeSubscription.Plan.MaxProfiles;

            var currentProfileCount = user.Profiles.Count;
            if (currentProfileCount >= maxProfiles)
            {
                var planName = activeSubscription?.Plan.DisplayName ?? "your current plan";
                throw new InvalidOperationException(
                    $"You have reached the maximum number of profiles ({maxProfiles}) " +
                    $"allowed by {planName}. " +
                    "Please upgrade your plan to add more profiles.");
            }


            string? pinHash = null;
            if (!string.IsNullOrWhiteSpace(request.PinHash))
            {
                pinHash = BCrypt.Net.BCrypt.HashPassword(request.PinHash);
            }

            request.PinHash= pinHash;


            var profileToCreate = _mapper.Map<ProfileEntity>(request);
            profileToCreate.UserId = user.Id;
            profileToCreate.SetAge(request.Age); // enforces IsKidsMode via domain logic

            await _profileRepository.AddProfileAsync(profileToCreate);
            await _profileBaseRepository.SaveChangesAsync(cancellationToken);



            var roles = (await _userManager.GetRolesAsync(user)).ToList();

            var profileToken = _jwtTokenGeneration.GenerateProfileJwtToken(user, profileToCreate, roles);

            return new CreateProfileResponce
            {
                AccessToken = profileToken
            };

        }
    }
}
