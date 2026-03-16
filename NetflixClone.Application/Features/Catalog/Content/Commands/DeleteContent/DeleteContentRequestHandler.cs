using MediatR;
using Microsoft.Extensions.Logging;
using NetflixClone.Application.Persistence;
using ContentEntity = NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Content.Commands.DeleteContent;

public class DeleteContentRequestHandler : IRequestHandler<DeleteContentRequest, bool>
{
    private readonly IBaseRepository<ContentEntity> _contentRepo;

    public DeleteContentRequestHandler(
        IBaseRepository<ContentEntity> contentRepo)
    {
        _contentRepo = contentRepo;
    }

    public async Task<bool> Handle(DeleteContentRequest request, CancellationToken cancellationToken)
    {
        var content = await _contentRepo.GetByIdAsync(request.Id, cancellationToken);

        if (content is null)
        {
            
            return false;
        }

        await _contentRepo.DeleteAsync(content);
        await _contentRepo.SaveChangesAsync(cancellationToken);

        
        return true;
    }
}

