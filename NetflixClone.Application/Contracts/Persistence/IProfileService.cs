using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Contracts.Persistence
{
    public interface IProfileService
    {
        Task<int> GetProfileCountForUserAsync(string userId);
    }
}
