using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Profiles.Commands.CreateProfile
{
    public class CreateProfileRequest : IRequest<bool>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }

        public bool? IsKidsMode { get; set; } = false;

        public string? AvatarUrl { get; set; } = null;

        public string? PinHash { get; set; }

        public string? PreferredLanguage { get; set; } = "en";
    }
}
