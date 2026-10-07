using BulkiBook.Models;

namespace BulkiBook.Business.Interfaces
{
    public interface ICategoryServices
    {
        public Task<Category> GetCategoryByIdAsync(int id);
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task<Category> CreateCategoryAsync(Category category);
        public Task<Category> UpdateCategoryByIdAsync(Category category);
        public Task<Category> DeleteCategoryByIdAsync(int id);
        public Task<bool> IsCategoryNameExistsAsync(string categoryName, int? CategoryId = null);
    }
}
