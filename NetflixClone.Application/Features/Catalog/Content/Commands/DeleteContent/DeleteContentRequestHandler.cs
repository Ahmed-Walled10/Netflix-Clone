using MediatR;
using NetflixClone.Application.Persistence;
using ContentEntity = NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Content.Commands.DeleteContent;

public class DeleteContentRequestHandler : IRequestHandler<DeleteContentRequest, Unit>
{
    private readonly IBaseRepository<ContentEntity> _contentRepo;

    public DeleteContentRequestHandler(
        IBaseRepository<ContentEntity> contentRepo)
    {
        _contentRepo = contentRepo;
    }

    public async Task<Unit> Handle(DeleteContentRequest request, CancellationToken cancellationToken)
    {
        var content = await _contentRepo.GetByIdAsync(request.Id, cancellationToken);

        if (content is null)
            throw new KeyNotFoundException($"Content {request.Id} was not found.");

        await _contentRepo.DeleteAsync(content, cancellationToken);
        await _contentRepo.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
