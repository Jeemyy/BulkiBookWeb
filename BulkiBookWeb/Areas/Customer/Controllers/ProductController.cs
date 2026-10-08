using BulkiBook.Business.Interfaces;
using BulkyBook.Models;
using BulkyBook.Models.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkiBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        public ProductController(IProductServices product, ICategoryServices category)
        {
            _productServices = product;
            _categoryServices = category;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetAllProductsAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryServices.GetAllCategoriesAsync();
            ProductVM productVm = new()
            {
                CategoryList = categories.Select(c => new SelectListItem 
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Product = new Product()
            };
            return View(productVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost(ProductVM productVm, IFormFile imageFile)
        {
            if (!string.IsNullOrEmpty(productVm.Product.Title) &&
                await _productServices.IsProductNameIsUniqueAsync(productVm.Product.Title))
            {
                ModelState.AddModelError(
                    "Product.Title",
                    "A product with this title already exists.");
            }

            if (ModelState.IsValid)
            {
                await _productServices.CreateProductAsync(productVm.Product);

                TempData["success"] = "Create product successfully.";

                return RedirectToAction("Index");
            }

            // IMPORTANT: rebuild CategoryList
            var categories = await _categoryServices.GetAllCategoriesAsync();

            productVm.CategoryList = categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(productVm);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var product = await _productServices.GetProductByIdAsync(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(Product product)
        {
            if(product is null)
            {
                return NotFound();
            }
            await _productServices.DeleteProductByIdAsync(product.Id);
            TempData["Success"] = "Delete Product Successfully";
            return RedirectToAction("Index");
        }

        /// <summary>
        ///     Region For API Calls For Product Controller
        /// </summary>
        public async Task<IActionResult> getAll(Product product)
        {
            var products = await _productServices.GetAllProductsAsync();
            return Json(new { data = products });
        }

    }
}
