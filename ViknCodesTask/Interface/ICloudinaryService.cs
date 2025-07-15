namespace ViknCodesTask.Interface
{
    public interface ICloudinaryService
    {
        Task<string> UploadImage(IFormFile Image);
    }

}
