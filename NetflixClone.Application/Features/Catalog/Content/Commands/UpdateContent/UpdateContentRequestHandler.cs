using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using ContentEntity = NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.UpdateContent
{
    public class UpdateContentRequestHandler : IRequestHandler<UpdateContentCommand, bool>
    {
        private readonly IBaseRepository<ContentEntity> _contentBaseRepository;

        public UpdateContentRequestHandler(IBaseRepository<ContentEntity> contentBaseRepository)
        {
            _contentBaseRepository = contentBaseRepository;
        }

        public async Task<bool> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
        {
            var content = await _contentBaseRepository.GetByIdAsync(request.ContentId);

            if (content == null)
                throw new KeyNotFoundException($"Content with Id {request.ContentId} not found.");

            content.Update(request.Data);

            await _contentBaseRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
