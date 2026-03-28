using NetflixClone.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Contracts.Persistence
{
    public interface IProfileRepository
    {
        Task<ApplicationUser> GetUserWithSubscriptionsAsync(string userId);
        Task<ApplicationUser> GetUserWithProfilesAsync(string userId);
        Task AddProfileAsync(Profile profile);

    }
}
