using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Application.Contracts.Infrasructure
{
    public interface ICloudinaryService
    {
        /// <summary>
        /// Uploads a video file to Cloudinary. Uses chunked upload (UploadLargeAsync) to handle large files.
        /// </summary>
        Task<CloudinaryUploadResult> UploadVideoAsync(Stream fileStream, string fileName, string folder);

        /// <summary>
        /// Uploads an image file to Cloudinary.
        /// </summary>
        Task<CloudinaryUploadResult> UploadImageAsync(Stream fileStream, string fileName, string folder);

        /// <summary>
        /// Builds a Cloudinary delivery URL for a video, constrained to the given quality tier.
        /// HD_720p → h_720, FullHD_1080p → h_1080, UHD_4K → h_2160.
        /// </summary>
        string GetVideoUrl(string publicId, VideoQuality quality);

        /// <summary>
        /// Deletes an asset from Cloudinary by its public ID.
        /// </summary>
        Task<bool> DeleteAssetAsync(string publicId, string resourceType);
    }

    public record CloudinaryUploadResult(string PublicId, string SecureUrl);
}
