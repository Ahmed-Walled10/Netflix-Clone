
using MediatR;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.MakeContentAvailable
{
    public class MakeContentAvaliableRequest : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
