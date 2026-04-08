using MediatR;
using NetflixClone.Domain.Entities.Catalog;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UpdateContent
{
    public class UpdateContentCommand : IRequest<bool>
    {
        public Guid ContentId { get; set; }
        public UpdateContentData Data { get; set; } = new();
    }
}
