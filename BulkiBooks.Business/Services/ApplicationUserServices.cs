using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Data;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkiBook.Business.Services
{
    public class ApplicationUserServices : IApplicationUserServices
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
