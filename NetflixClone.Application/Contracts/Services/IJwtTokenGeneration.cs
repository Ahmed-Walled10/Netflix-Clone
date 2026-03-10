using NetflixClone.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Contracts
{
    public interface IJwtTokenGeneration
    {
        public string GenerateJwtToken(ApplicationUser user, List<string> roles);

    }
}
