using MediatR;

namespace NetflixClone.Application.Features.Profiles.Commands.DeleteProfile;

public class DeleteProfileRequest : IRequest<Unit>
{
    /// <summary>Authenticated user's ID. Set by the controller from JWT claims.</summary>
    public string UserId { get; set; } = string.Empty;

    public Guid ProfileId { get; set; }
}
