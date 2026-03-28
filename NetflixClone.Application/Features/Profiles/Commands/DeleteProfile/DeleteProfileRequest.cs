using MediatR;

namespace NetflixClone.Application.Features.Profiles.Commands.DeleteProfile;

public class DeleteProfileRequest : IRequest<Unit>
{
    public Guid ProfileId { get; set; }
}
