using MediatR;

namespace NetflixClone.Application.Features.Catalog.Person.Commands.UploadPersonPhoto;

/// <summary>
/// Uploads a photo for a Person (actor, director, etc.).
/// </summary>
public class UploadPersonPhotoRequest : IRequest<UploadPersonPhotoResponse>
{
    public Guid PersonId { get; set; }
    public Stream FileStream { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
}
