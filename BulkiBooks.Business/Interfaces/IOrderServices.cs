using BulkyBook.Models;
namespace BulkiBook.Business.Interfaces
{
    public  interface IOrderServices
    {
        Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader);
        Task<OrderHeader?> GetOrderByIdAsync(int orderId, bool includeUser = false, bool includeDetails = false);
        Task<IEnumerable<OrderHeader>> GetAllOrderAsync(string? userId = null, string? status = null, bool includeUser = false, bool includeDetails = false);
        Task UpdateOrderAsync(OrderHeader orderHeader);
        Task RemoveOrderAsync(int id);
        Task UpdateOrderStatusAsync(int id, string orderStatus, string? carrier = null, string? trackingNumber = null);
    }
}
