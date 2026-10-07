using BulkiBook.Business.Interfaces;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Helpers;

namespace BulkiBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        public ProductController(IProductServices product)
        {
            _productServices = product;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetAllProductsAsync();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost(Product product)
        {
            if (!string.IsNullOrEmpty(product.Title) && await _productServices.IsProductNameIsUniqueAsync(product.Title))
            {
                ModelState.AddModelError("Title", "A product with this title already exists.");
            }
            if (ModelState.IsValid)
            {
                await _productServices.CreateProductAsync(product);
                TempData["Success"] = "Create product Successfully.";
                return RedirectToAction("Index");
            }
            return View();
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
