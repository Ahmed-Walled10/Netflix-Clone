using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Profiles.Commands.UpdateProfile
{
    public class UpdateProfileRequest
    {
        [StringLength(20)]
        public string? Name { get; set; }
        
        [Range(0, 120)]
        public int? Age { get; set; }
        
        public string? AvatarUrl { get; set; }
        
        [StringLength(6, MinimumLength = 6)]
        public string? PinHash { get; set; }
        
        [StringLength(10)]
        public string? PreferredLanguage { get; set; }
    }
}
