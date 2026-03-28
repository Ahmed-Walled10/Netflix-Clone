using MediatR;

namespace NetflixClone.Application.Features.Streaming.Queries.PlayContent;

/// <summary>
/// Requests a streaming URL for a Content item (movie) or a specific Episode (series).
/// The returned URL quality is capped by the user's subscription plan.
/// </summary>
public class PlayContentRequest : IRequest<PlayContentResponse>
{
    public Guid ContentId { get; set; }
    public Guid? EpisodeId { get; set; }
    public Guid ProfileId { get; set; }
    public string UserId { get; set; } = string.Empty;
}
