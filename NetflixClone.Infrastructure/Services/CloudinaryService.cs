using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Domain.Common.Enums;
using NetflixClone.Infrastructure.Options;

namespace NetflixClone.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(IOptions<CloudinaryOptions> options, ILogger<CloudinaryService> logger)
        {
            var opts = options.Value;
            _logger = logger;

            var account = new Account(opts.CloudName, opts.ApiKey, opts.ApiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        // ── Upload Video ─────────────────────────────────────────────────────────
        public async Task<CloudinaryUploadResult> UploadVideoAsync(Stream fileStream, string fileName, string folder)
        {
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            // UploadLargeAsync supports chunked uploads for files > 100 MB
            var result = await _cloudinary.UploadLargeAsync(uploadParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError("Cloudinary video upload failed: {Error}", result.Error?.Message);
                throw new InvalidOperationException($"Cloudinary video upload failed: {result.Error?.Message}");
            }

            _logger.LogInformation("Video uploaded to Cloudinary: {PublicId}", result.PublicId);
            return new CloudinaryUploadResult(result.PublicId, result.SecureUrl.ToString());
        }

        // ── Upload Image ─────────────────────────────────────────────────────────
        public async Task<CloudinaryUploadResult> UploadImageAsync(Stream fileStream, string fileName, string folder)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError("Cloudinary image upload failed: {Error}", result.Error?.Message);
                throw new InvalidOperationException($"Cloudinary image upload failed: {result.Error?.Message}");
            }

            _logger.LogInformation("Image uploaded to Cloudinary: {PublicId}", result.PublicId);
            return new CloudinaryUploadResult(result.PublicId, result.SecureUrl.ToString());
        }

        // ── Get Video URL with Quality ───────────────────────────────────────────
        public string GetVideoUrl(string publicId, VideoQuality quality)
        {
            int height = quality switch
            {
                VideoQuality.HD_720p     => 720,
                VideoQuality.FullHD_1080p => 1080,
                VideoQuality.UHD_4K      => 2160,
                _ => 1080
            };

            var url = _cloudinary.Api.UrlVideoUp
                .Transform(new Transformation().Height(height).Crop("limit"))
                .BuildUrl(publicId);

            return url;
        }

        // ── Delete Asset ─────────────────────────────────────────────────────────
        public async Task<bool> DeleteAssetAsync(string publicId, string resourceType)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = resourceType.ToLower() switch
                {
                    "video" => ResourceType.Video,
                    "image" => ResourceType.Image,
                    _ => ResourceType.Image
                }
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Result == "ok")
            {
                _logger.LogInformation("Cloudinary asset deleted: {PublicId}", publicId);
                return true;
            }

            _logger.LogWarning("Cloudinary asset deletion failed for {PublicId}: {Result}", publicId, result.Result);
            return false;
        }
    }
}
