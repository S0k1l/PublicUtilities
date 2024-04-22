using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using PublicUtilities.Interface;
using PublicUtilities.Helpers;
using System.Text.RegularExpressions;
using System.Security.Policy;

namespace PublicUtilities.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "PublicUtilities",
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string imageUrl)
        {
            var publicId = ExtractPublicId(imageUrl);

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        public string ExtractPublicId(string imageUrl)
        {
            var segments = imageUrl.Split('/');
            var publicId = $"{segments[segments.Length - 2]}/{segments[segments.Length - 1].Split('.')[0]}"; // Public ID is usually the second-to-last segment
            return publicId;
        }
    }
}
