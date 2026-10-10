using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Data;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BulkiBook.Business.Services
{
    public class ProductServices : IProductServices
    {
        private readonly ApplicationDbContext _context;
        public ProductServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                throw new ValidationException("Product not found.");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool includeCategory = true)
        {
            if (includeCategory)
            {
                return await _context.Products.Include(p => p.Category).ToListAsync();
            }
            else
            {
                return await _context.Products.ToListAsync();
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                throw new ValidationException("Product not found.");
            }
            return product;
        }

        public async Task<bool> IsProductNameIsUniqueAsync(string productName, int? productId = null)
        {
            if (productId.HasValue)
            {
                return await _context.Products.AnyAsync(c => c.Title.ToLower() == productName.ToLower()
                && c.Id != productId);
            }
            else
            {
                return await _context.Products.AnyAsync(c => c.Title.ToLower() == productName.ToLower());
            }
        }

        public async Task<Product> UpdateProductByIdAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
