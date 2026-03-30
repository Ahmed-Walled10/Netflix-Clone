using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Contracts.Infrasructure
{
    public interface IJwtTokenGeneration
    {
        string GenerateJwtToken(ApplicationUser user, List<string> roles);

        string GenerateProfileJwtToken(ApplicationUser user, Profile profile, List<string> roles);


    }
}
