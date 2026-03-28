using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Engagement.Commands.AddRating
{
    public class AddRatingRequest : IRequest<AddRatingResponse>
    {
        /// <summary>The Id of the content (movie/series) being rated.</summary>
        [Required]
        public Guid ContentId { get; set; }


        //for the rating
        [Required]
        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
        public int Value { get; set; }

        public string? Review { get; set; }
    }
}
