using MediatR;

namespace NetflixClone.Application.Features.Content.Commands.DeleteContent;

public class DeleteContentRequest : IRequest<Unit>
{
    public Guid Id { get; set; }
}
