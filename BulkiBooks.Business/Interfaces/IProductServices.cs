using BulkyBook.Models;

namespace BulkiBook.Business.Interfaces
{
    public interface IProductServices
    {
        public Task<Product> GetProductByIdAsync(int id, bool includeCategory = false);
        public Task<IEnumerable<Product>> GetAllProductsAsync(bool includeCategory=true);
        public Task<Product> CreateProductAsync(Product product);
        public Task<Product> UpdateProductByIdAsync(Product product);
        public Task<Product> DeleteProductByIdAsync(int id);
        public Task<bool> IsProductNameIsUniqueAsync(string productName, int? productId = null);
    }
}
