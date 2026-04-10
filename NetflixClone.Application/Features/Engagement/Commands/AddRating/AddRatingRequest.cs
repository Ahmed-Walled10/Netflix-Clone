using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Engagement.Commands.AddRating
{
    public class AddRatingRequest : IRequest<AddRatingResponse>
    {
        /// <summary>The Id of the content (movie/series) being rated.</summary>
        public Guid ContentId { get; set; }

        [Required]
        public Guid ProfileId { get; set; }

        public int Value { get; set; }

        public string? Review { get; set; }
    }
}
