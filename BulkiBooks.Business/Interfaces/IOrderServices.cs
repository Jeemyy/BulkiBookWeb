using BulkyBook.Models;
namespace BulkiBook.Business.Interfaces
{
    public  interface IOrderServices
    {
        Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader);
        Task<OrderHeader?> GetOrderByIdAsync(int orderId, bool includeUser = false, bool includeDetails = false);
        Task<IEnumerable<OrderHeader>> GetAllOrderAsync(string? userId = null, string? status = null, bool includeUser = false, bool includeDetails = false);
    }
}
