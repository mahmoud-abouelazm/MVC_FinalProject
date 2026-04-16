namespace Library.Web.Services.HelperServices
{
    public interface IImageService
    {
        Task<string?> SaveAsync(IFormFile? file);
    }
}
