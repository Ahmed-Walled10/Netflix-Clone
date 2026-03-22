namespace NetflixClone.Application.ResourceParameters
{
    public class RatingsResourceParameters : BaseResourceParameters
    {
        /// <summary>Filter ratings to a specific content item.</summary>
        public Guid? ContentId { get; set; }

        public bool? OrderedByDate { get; set; } = false;

        /// <summary>Order ratings from highest to lowest when true.</summary>
        public bool? OrderedByRatingDescending { get; set; } = true;
    }
}
