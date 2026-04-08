namespace NetflixClone.Application.Features.Engagement.Commands.UpdateRating
{
    public class UpdateRatingRequest
    {
        public int? Value { get; set; }
        public string? Review { get; set; }
    }
}
