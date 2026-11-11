using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Data;
using BulkiBook.Utility;
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
            if (!string.IsNullOrEmpty(status) && status.ToLower() != "all")
            {
                query = query.Where(q => q.OrderStatus.ToLower() == status.ToLower());
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
        public async Task UpdateOrderAsync(OrderHeader orderHeader) 
        {
            _db.OrderHeaders.Update(orderHeader);
            await _db.SaveChangesAsync();
        }
        public async Task RemoveOrderAsync(int id)
        {
            var order = await GetOrderByIdAsync(id);
            _db.OrderHeaders.Remove(order);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusAsync(int id, string orderStatus, string? carrier = null, string? trackingNumber = null)
        {
            var order = await _db.OrderHeaders.FindAsync(id);
            if (order is null)
            {
                throw new KeyNotFoundException("Order is not found!");
            }
            order.OrderStatus = orderStatus;
            if (orderStatus == SD.StatusShipped)
            {
                order.OrderShipping = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(carrier))
                {
                    order.Carrier = carrier;
                }
                if (!string.IsNullOrEmpty(trackingNumber))
                {
                    order.TrackingNumber = trackingNumber;
                }
            }
            await _db.SaveChangesAsync();
        }
    }
}
