using MediatR;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Application.Contracts.Persistence;
using PersonEntity = NetflixClone.Domain.Entities.Catalog.Person;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.UploadPersonPhoto;

public class UploadPersonPhotoRequestHandler
    : IRequestHandler<UploadPersonPhotoRequest, UploadPersonPhotoResponse>
{
    private readonly IBaseRepository<PersonEntity> _personRepo;
    private readonly ICloudinaryService _cloudinaryService;

    public UploadPersonPhotoRequestHandler(
        IBaseRepository<PersonEntity> personRepo,
        ICloudinaryService cloudinaryService)
    {
        _personRepo = personRepo;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<UploadPersonPhotoResponse> Handle(
        UploadPersonPhotoRequest request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepo.GetByIdAsync(request.PersonId, cancellationToken)
            ?? throw new KeyNotFoundException($"Person {request.PersonId} not found.");

        var result = await _cloudinaryService.UploadImageAsync(
            request.FileStream,
            request.FileName,
            $"netflix-clone/persons/{request.PersonId}");

        person.PhotoUrl = result.SecureUrl;

        await _personRepo.UpdateAsync(person);
        await _personRepo.SaveChangesAsync(cancellationToken);

        return new UploadPersonPhotoResponse
        {
            PhotoUrl = result.SecureUrl
        };
    }
}
