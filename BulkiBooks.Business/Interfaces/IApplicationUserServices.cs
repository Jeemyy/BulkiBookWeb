using BulkyBook.Models;
using BulkyBook.Models.ModelViews;

namespace BulkiBook.Business.Interfaces
{
    public interface IApplicationUserServices
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetAllUserAsync();
        Task<ApplicationUser> FindByIdAsync(int id);
    }
}
