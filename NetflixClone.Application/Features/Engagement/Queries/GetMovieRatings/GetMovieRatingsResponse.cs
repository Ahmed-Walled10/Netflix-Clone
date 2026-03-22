
namespace NetflixClone.Application.Features.Engagement.Queries.GetMovieRatings
{
    public class GetMovieRatingsResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int RatingValue { get; set; }
        public string Review { get; set; } = string.Empty;
        public DateTime RatedAt { get; set; }
    }
}
