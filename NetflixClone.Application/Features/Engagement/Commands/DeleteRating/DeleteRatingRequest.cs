using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Features.Engagement.Commands.DeleteRating
{
    public class DeleteRatingRequest : IRequest<bool>
    {
         public Guid RatingId { get; set; }
    }
}
