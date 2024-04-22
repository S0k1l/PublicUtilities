using CloudinaryDotNet.Actions;

namespace PublicUtilities.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string imageUrl);

    }
}
