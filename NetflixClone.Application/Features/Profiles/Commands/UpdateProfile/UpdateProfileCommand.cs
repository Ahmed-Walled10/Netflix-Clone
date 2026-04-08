using MediatR;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Profiles.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<bool>
    {
        public Guid ProfileId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public UpdateProfileData Data { get; set; } = new();
    }
}
