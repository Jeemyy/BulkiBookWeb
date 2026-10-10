using Microsoft.AspNetCore.Http;

namespace BulkiBook.Business.Interfaces
{
    public interface IFileServices
    {
        public Task<string> CreateFileImageAsync(IFormFile imageFile);
    }
}