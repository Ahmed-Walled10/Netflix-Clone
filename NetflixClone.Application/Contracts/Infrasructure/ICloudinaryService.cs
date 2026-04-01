using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Application.Contracts.Infrasructure
{
    public interface ICloudinaryService
    {

        Task<CloudinaryUploadResult> UploadVideoAsync(Stream fileStream, string fileName, string folder);

        Task<CloudinaryUploadResult> UploadImageAsync(Stream fileStream, string fileName, string folder);

        string GetVideoUrl(string publicId, VideoQuality quality);

        string GetHlsUrl(string publicId, VideoQuality quality);

        Task<bool> DeleteAssetAsync(string publicId, string resourceType);
    }

    public record CloudinaryUploadResult(string PublicId, string SecureUrl);
}
