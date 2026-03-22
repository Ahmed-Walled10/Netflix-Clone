namespace NetflixClone.Application.Features.Engagement.Queries.GetMyMovieRating
{
    public class GetMyMovieRatingResponse
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
        public int Value { get; set; }
        public string? Review { get; set; }
        public DateTime RatedAt { get; set; }
    }
}
