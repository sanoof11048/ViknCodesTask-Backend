using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System.Net;
using ViknCodesTask.Interface;
using ViknCodesTask.Models;

namespace ViknCodesTask.Service
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> UploadImage(IFormFile Image)
        {
            if(Image == null || Image.Length == 0)
                throw new ArgumentNullException("Invalid File");

            await using var stream = Image.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(Image.FileName, stream),
                Folder = "products",
                PublicId = Guid.NewGuid().ToString()
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != HttpStatusCode.OK)
                throw new Exception("Cloudinary upload failed");

            using var httpClient = new HttpClient();
            return result.SecureUrl.ToString();
        }
    }
}
