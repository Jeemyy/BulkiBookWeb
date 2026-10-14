using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Data;
using BulkiBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkiBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryServices _categoryService;
        public CategoryController(ApplicationDbContext context, ICategoryServices categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost(Category category)
        {
            var categoryName = category.Name is null ? "" : category.Name.ToLower();
            var categoryNameIsExists = await _categoryService.IsCategoryNameExistsAsync(category.Name);
            if (!string.IsNullOrEmpty(category.Name) && categoryNameIsExists)
            {
                ModelState.AddModelError("", "Category name is already exists!");
            }
            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(category);
                TempData["success"] = "Create category successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            var categoryId = await _categoryService.GetCategoryByIdAsync(id.Value);
            return View(categoryId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public async Task<IActionResult> UpdatePost(Category category)
        {
            var categoryName = await _categoryService.IsCategoryNameExistsAsync(category.Name, category.Id);
            if (!string.IsNullOrEmpty(category.Name) && categoryName)
            {
                ModelState.AddModelError("", "Category name is already exists!");
            }
            if(ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryByIdAsync(category);
                TempData["success"] = "Category updated successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(Category category)
        {
            if (category is null) {
                return NotFound();
            }
            await _categoryService.DeleteCategoryByIdAsync(category.Id);
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }


        /// <summary>
        ///     Region For API Calls For Category Controller
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>

        public async Task<IActionResult> GetAll(Category category)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Json(new { data = categories });
        }

    }
}
