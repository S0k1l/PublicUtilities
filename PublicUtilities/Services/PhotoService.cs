using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using PublicUtilities.Interface;
using PublicUtilities.Helpers;

namespace PublicUtilities.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloundinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloundinary = new Cloudinary(acc);
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
                uploadResult = await _cloundinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }
    }
}
