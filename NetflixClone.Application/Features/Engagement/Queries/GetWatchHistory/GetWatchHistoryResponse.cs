namespace NetflixClone.Application.Features.Engagement.Queries.GetWatchHistory
{
    public class GetWatchHistoryResponse
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
        public string ContentTitle { get; set; } = string.Empty;
        public string? ContentThumbnailUrl { get; set; }

        /// <summary>Null for movies; set for series episodes.</summary>
        public Guid? EpisodeId { get; set; }

        /// <summary>Playback position in seconds when the user last stopped.</summary>
        public int StoppedAtSeconds { get; set; }

        /// <summary>True if the user has watched at least 90% of the content.</summary>
        public bool IsCompleted { get; set; }

        public DateTime WatchedAt { get; set; }
    }
}
