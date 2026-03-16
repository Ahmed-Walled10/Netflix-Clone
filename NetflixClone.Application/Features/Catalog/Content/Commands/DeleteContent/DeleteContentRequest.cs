using MediatR;

namespace NetflixClone.Application.Features.Content.Commands.DeleteContent;

public class DeleteContentRequest : IRequest<bool>
{
    public Guid Id { get; set; }
}

