using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Data;
using BulkyBook.Models;
using BulkyBook.Models.ModelViews;
using Microsoft.EntityFrameworkCore;

namespace BulkiBook.Business.Services
{
    public class ApplicationUserServices : IApplicationUserServices
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserServices(ApplicationDbContext context)
        {
            _db = context;
        }

        public Task<ApplicationUser> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUserAsync()
        {
            return await _db.ApplicationUsers.ToListAsync();
        }

        public Task<RoleManagementVM> GetRolesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
