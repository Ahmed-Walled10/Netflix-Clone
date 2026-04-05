using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Contracts.Persistence
{
    public interface IProfileRepository
    {
        Task<ApplicationUser> GetUserWithSubscriptionsAsync(string userId);
        Task<ApplicationUser> GetUserWithProfilesAsync(string userId);
        Task<ApplicationUser> GetUserWithSubscriptionsAndProfilesAsync(string userId);
        Task AddProfileAsync(Profile profile);

    }
}
