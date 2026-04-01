using AutoMapper;
using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using ContentEntity=NetflixClone.Domain.Entities.Catalog.Content;

namespace NetflixClone.Application.Features.Catalog.Content.Queries.GetContentById
{
    public class GetContentByIdRequestHandler : IRequestHandler<GetContentByIdRequest, GetContentByIdResponse>
    {
        private readonly IBaseRepository<ContentEntity> _contentRepository;
        private readonly IMapper _mapper;

        public GetContentByIdRequestHandler(
            IBaseRepository<ContentEntity> contentRepository,
            IMapper mapper)
        {
            _contentRepository = contentRepository;
            _mapper = mapper;
        }

        public async Task<GetContentByIdResponse> Handle(GetContentByIdRequest request, CancellationToken cancellationToken)
        {
            var content = await _contentRepository.GetByIdAsync(request.Id, cancellationToken);
            var ContentMaturityRating = (int)content.MaturityRating;
            if (content == null || (ContentMaturityRating<=13 && request.IsUserKid==true))
            {
                throw new KeyNotFoundException($"Content with Id {request.Id} was not found.");
            }

            return _mapper.Map<GetContentByIdResponse>(content);
        }
    }
}
