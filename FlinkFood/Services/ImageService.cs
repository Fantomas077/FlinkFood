using Microsoft.AspNetCore.Components.Forms;

namespace FlinkFood.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _web;

        public ImageService(IWebHostEnvironment web)
        {
            _web = web;
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            var folder = Path.Combine(_web.WebRootPath, "images", "product");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
            var path = Path.Combine(folder, fileName);

            using var stream = File.Create(path);
            await file.OpenReadStream(maxAllowedSize: 5_000_000).CopyToAsync(stream);

            return fileName;
        }

        public void DeleteImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            var path = Path.Combine(_web.WebRootPath, "images", "product", fileName);

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
