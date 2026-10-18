using BulkiBook.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace BulkiBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IProductServices _productServices;
        public HomeController(IProductServices productServices)
        {
            _productServices = productServices;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetAllProductsAsync();
            return View(products);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productServices.GetProductByIdAsync(id);
            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
