namespace NetflixClone.Application.Features.Streaming.Queries.PlayContent;

public class PlayContentResponse
{
    public string StreamingUrl { get; set; } = string.Empty;
    public string ManifestUrl { get; set; } = string.Empty;
    public string Protocol { get; set; } = "HLS";
    public string Quality { get; set; } = string.Empty;
}
