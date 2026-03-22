namespace NetflixClone.Application.Features.Engagement.Commands.AddRating
{
    public class AddRatingResponse
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
        public Guid ProfileId { get; set; }
        public int Value { get; set; }
        public string? Review { get; set; }
        public DateTime RatedAt { get; set; }
    }
}
