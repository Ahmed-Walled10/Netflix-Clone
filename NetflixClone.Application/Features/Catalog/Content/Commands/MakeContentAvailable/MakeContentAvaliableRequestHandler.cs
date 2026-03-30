/*using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using ContentEntity = NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Catalog.Content.Commands.MakeContentAvailable
{
    public class MakeContentAvaliableRequestHandler : IRequestHandler<MakeContentAvaliableRequest, bool>
    {
        private readonly IBaseRepository<ContentEntity> _contentRepo;
        public MakeContentAvaliableRequestHandler(IBaseRepository<ContentEntity> contentRepo)
        {
            _contentRepo = contentRepo;
        }
        public async Task<bool> Handle(MakeContentAvaliableRequest request, CancellationToken cancellationToken)
        {
            var content = await _contentRepo.GetByIdAsync(request.Id, cancellationToken);
            if (content is null)
                return false;

            content.IsAvailable = true;
            await _contentRepo.UpdateAsync(content);
            await _contentRepo.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}*/
