using BulkiBook.Business.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BulkiBook.Business.Services
{
    public class FileServices : IFileServices
    {
        private readonly IWebHostEnvironment _webHost;

        public FileServices(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public async Task<string> CreateFileImageAsync(IFormFile imageFile)
        {
            var wwwRootPath = _webHost.WebRootPath;
            if (imageFile is null || imageFile.Length == 0)
            {
                throw new ArgumentNullException("Image file cannot be null or empty.");
            }

            if (wwwRootPath is null)
            {
                throw new ArgumentNullException(nameof(wwwRootPath), "WWW root path cannot be null.");
            }

            var fileSizeLimit = 5 * 1024 * 1024; // 5 MB

            if (imageFile.Length > fileSizeLimit)
            {
                throw new InvalidOperationException("File size exceeds the limit of 5 MB.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var fileExtension = Path.GetExtension(imageFile.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            if(!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new InvalidOperationException("Invalid file extension. Only .jpg, .jpeg, .png, and .webp are allowed.");
            }

            var productPath = Path.Combine("images", "products");
            var finalPath = Path.Combine(wwwRootPath, productPath);

            Directory.CreateDirectory(finalPath);
            var filePath = Path.Combine(finalPath, fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/products/{fileName}";
        }
    }
}
