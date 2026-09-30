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
            var categoryName = category.Name is null ? "": category.Name.ToLower();
            var categoryNameIsExists = _context.Categories.Any(c => c.Name.ToLower() == categoryName);
            if (!string.IsNullOrEmpty(category.Name) && categoryNameIsExists)
            {
                ModelState.AddModelError("", "Category name is already exists!");
            }
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
