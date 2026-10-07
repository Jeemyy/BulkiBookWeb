using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Data;
using BulkiBook.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BulkiBook.Business.Services
{
    internal class CategoryServices : ICategoryServices
    {
        private readonly ApplicationDbContext _context;
        public CategoryServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> DeleteCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category is null)
            {
                throw new ValidationException("Category not found");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> IsCategoryNameExistsAsync(string categoryName, int? CategoryId = null)
        {
            if (CategoryId.HasValue)
            {
                return await _context.Categories.AnyAsync(c => c.Name.ToLower() == categoryName.ToLower()
                && c.Id != CategoryId.Value);
            }
            else
            {
                return await _context.Categories.AnyAsync(c => c.Name.ToLower() == categoryName.ToLower());
            }
        }

        public async Task<Category> UpdateCategoryByIdAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

    }
}
