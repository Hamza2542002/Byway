using Microsoft.AspNetCore.Http;

namespace Byway.Core.IServices;

public interface IImageService
{
    Task<string?> UploadImageAsync(IFormFile? image, Guid id);
    Task<string?> UpdateImageAsync(IFormFile? image, Guid id);
    Task<bool> DeleteImageAsync(Guid id);
}
