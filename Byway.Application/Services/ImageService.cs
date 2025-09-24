using Byway.Core.Helpers;
using Byway.Core.IServices;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Byway.Application.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(IOptions<CloudinatuConfiguration> cloudinaryOptions)
    {
        var account = new Account(
            cloudinaryOptions.Value.CloudName,
            cloudinaryOptions.Value.ApiKey,
            cloudinaryOptions.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(account);
    }
    public async Task<string?> UploadImageAsync(IFormFile image, Guid id)
    {
        using var stream = image.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(image.FileName, stream),
            PublicId = $"{id.ToString()}"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }
    public async Task<bool> DeleteImageAsync(Guid id)
    {
        var delteResult = await _cloudinary.DestroyAsync(new DeletionParams(id.ToString()));

        if (delteResult.StatusCode == System.Net.HttpStatusCode.OK)
            return true;

        return false;
    }
    public async Task<string?> UpdateImageAsync(IFormFile? image, Guid id)
    {
        if (id == Guid.Empty || image == null)
            return null;

        await DeleteImageAsync(id);

        return await UploadImageAsync(image, id);
    }
}
