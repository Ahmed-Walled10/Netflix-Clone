using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Profiles.Commands.LoginToProfile
{
    public class LoginToProfileRequest : IRequest<LoginToProfileResponce>
    {
        public string UserId { get; set; } = string.Empty;

        public string ProfileId { get; set; }


        [StringLength(6, MinimumLength = 6)]
        public string? Pin { get; set; }

    }
}
