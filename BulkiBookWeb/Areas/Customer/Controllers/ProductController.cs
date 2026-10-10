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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileServices _fileServices;
        public ProductController(IProductServices product, ICategoryServices category, IWebHostEnvironment webHost, IFileServices fileServices)
        {
            _productServices    = product;
            _categoryServices   = category;
            _webHostEnvironment = webHost;
            _fileServices = fileServices;
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
        public async Task<IActionResult> CreatePost(ProductVM productVm, IFormFile? imageFile)
        {
            var productTitleUnique = await _productServices.IsProductNameIsUniqueAsync(productVm.Product.Title);
            if (!string.IsNullOrEmpty(productVm.Product.Title) && productTitleUnique)
            {
                ModelState.AddModelError("", "Category name is already exists!");
            }
            if (ModelState.IsValid)
            {
                if(imageFile is not null && imageFile.Length > 0)
                {
                    productVm.Product.ImageUrl = await _fileServices.CreateFileImageAsync(imageFile);
                }

                await _productServices.CreateProductAsync(productVm.Product);
                TempData["Success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                var categories = await _categoryServices.GetAllCategoriesAsync();
                productVm = new()
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
