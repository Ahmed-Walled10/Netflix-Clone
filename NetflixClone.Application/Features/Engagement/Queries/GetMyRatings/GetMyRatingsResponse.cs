namespace NetflixClone.Application.Features.Engagement.Queries.GetMyRatings
{
    public class GetMyRatingsResponse
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
        public string ContentTitle { get; set; } = string.Empty;
        public string? ContentThumbnailUrl { get; set; }
        public int Value { get; set; }
        public string? Review { get; set; }
        public DateTime RatedAt { get; set; }
    }
}
