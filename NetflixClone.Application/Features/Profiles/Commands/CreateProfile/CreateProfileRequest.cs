using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Profiles.Commands.CreateProfile
{
    public class CreateProfileRequest : IRequest<CreateProfileResponce>
    {
        public string? UserId { get; set; }

        public string Name { get; set; }
        [Required]
        [Range(0, 120)]
        public int Age { get; set; }

        public bool? IsKidsMode { get; set; } = false;

        public string? AvatarUrl { get; set; } = null;

        public string? PinHash { get; set; }

        [StringLength(10)]
        public string? PreferredLanguage { get; set; } = "en";

    }
}
