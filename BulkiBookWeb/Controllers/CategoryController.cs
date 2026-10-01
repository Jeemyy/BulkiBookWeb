using BulkiBookWeb.Data;
using BulkiBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkiBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public IActionResult CreatePost(Category category)
        {
            var categoryName = category.Name is null ? "" : category.Name.ToLower();
            var categoryNameIsExists = _context.Categories.Any(c => c.Name.ToLower() == categoryName);
            if (!string.IsNullOrEmpty(category.Name) && categoryNameIsExists)
            {
                ModelState.AddModelError("", "Category name is already exists!");
            }
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                TempData["success"] = "Create category successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Update(int? id)
        {
            if (!_context.Categories.Any(c => c.Id == id) || id is null || id == 0)
            {
                return NotFound();
            }
            var categoryId = _context.Categories.Find(id);
            return View(categoryId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public IActionResult UpdatePost(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name) &&
                _context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower()
                && c.Id != category.Id))
            {
                ModelState.AddModelError("", "Category name is already exists!");
            }
            if(ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                TempData["success"] = "Category updated successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _context.Categories.Find(id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(Category category)
        {
            if (category is null) {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
