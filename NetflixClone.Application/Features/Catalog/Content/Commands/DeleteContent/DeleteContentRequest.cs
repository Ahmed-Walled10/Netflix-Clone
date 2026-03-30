using MediatR;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.DeleteContent;

public class DeleteContentRequest : IRequest<Unit>
{
    public Guid Id { get; set; }
}
