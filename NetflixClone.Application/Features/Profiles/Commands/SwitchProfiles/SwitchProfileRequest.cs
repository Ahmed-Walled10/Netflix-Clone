using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Profiles.Commands.SwitchProfile;

public class SwitchProfileRequest : IRequest<SwitchProfileResponse>
{
    /// <summary>Authenticated user's ID. Set by the controller from JWT claims.</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>The profile to switch into. Must belong to the authenticated account.</summary>
    public Guid ProfileId { get; set; }

    [StringLength(10)]
    public string? Pin { get; set; }
}