using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Engagement.Commands.AddRating
{
    public class AddRatingRequest : IRequest<AddRatingResponse>
    {
        /// <summary>The Id of the content (movie/series) being rated.</summary>
        [Required]
        public Guid ContentId { get; set; }

        /// <summary>
        /// The Id of the profile submitting the rating.
        /// Should be resolved from the authenticated user's claims in the API controller.
        /// </summary>
        [Required]
        public Guid ProfileId { get; set; }

        /// <summary>Rating value — must be between 1 and 5.</summary>
        [Required]
        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
        public int Value { get; set; }

        public string? Review { get; set; }
    }
}
