using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Data;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkiBook.Business.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _db;
        public OrderServices(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return orderHeader;
        }

        public async Task<IEnumerable<OrderHeader>> GetAllOrderAsync(string? userId = null, string? status = null, bool includeUser = false, bool includeDetails = false)
        {
            var query = _db.OrderHeaders.AsQueryable();
            if (includeUser)
            {
                query = query.Include(q => q.ApplicationUser);
            }
            if (includeDetails)
            {
                query = query
                    .Include(q => q.OrderDetails)
                        .ThenInclude(o => o.Product);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(q => q.ApplicationUserId == userId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(q => q.OrderStatus == status);
            }
            return await query.ToListAsync();
        }

        public async Task<OrderHeader?> GetOrderByIdAsync(int orderId, bool includeUser = false, bool includeDetails = false)
        {
            var query = _db.OrderHeaders.AsQueryable();
            if (includeUser)
            {
                query = query.Include(q => q.ApplicationUser);
            }
            if (includeDetails)
            {
                query = query
                    .Include(q => q.OrderDetails)
                        .ThenInclude(o => o.Product);
            }
            return await query.FirstOrDefaultAsync(q => q.Id == orderId);
        }
    }
}
