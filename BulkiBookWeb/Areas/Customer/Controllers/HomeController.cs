using BulkiBook.Business.Interfaces;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkiBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IShoppingCartServices _shoppingCartServices;
        public HomeController(IProductServices productServices, IShoppingCartServices shopping)
        {
            _productServices = productServices;
            _shoppingCartServices = shopping;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetAllProductsAsync();
            return View(products);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productServices.GetProductByIdAsync(id);

            if (product is null)
            {
                return NotFound();
            }
            ShoppingCart cart = new()
            {
                Product = product,
                Count = 1,
                ProductId = id
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart cart)
        {
            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIndentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            cart.ApplicationUserId = userId;
            await _shoppingCartServices.AddToCartAsync(cart);
            return RedirectToAction("Details", new { ProductId = cart.ProductId });
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
