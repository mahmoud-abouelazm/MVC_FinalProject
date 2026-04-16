namespace Library.Web.Services.HelperServices
{
    public class ImageService(IWebHostEnvironment env) : IImageService
    {
        private readonly IWebHostEnvironment _env = env;

        public async Task<string?> SaveAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var dir = Path.Combine(_env.WebRootPath, "uploads", "covers");
            Directory.CreateDirectory(dir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = new FileStream(Path.Combine(dir, fileName), FileMode.Create);
            await file.CopyToAsync(stream);

            return "/uploads/covers/" + fileName;
        }
    }
}

