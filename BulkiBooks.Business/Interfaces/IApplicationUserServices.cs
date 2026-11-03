using BulkyBook.Models;

namespace BulkiBook.Business.Interfaces
{
    public interface IApplicationUserServices
    {
        public Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}
