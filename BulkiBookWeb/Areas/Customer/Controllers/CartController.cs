using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Migrations;
using BulkyBook.Models.ModelViews;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkiBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IShoppingCartServices _shoppingCartServices;
        private readonly IApplicationUserServices _applicationUserServices;
        public CartController(IProductServices product, IShoppingCartServices shoppingCartServices, IApplicationUserServices applicationUserServices)
        {
            _productServices = product;
            _shoppingCartServices = shoppingCartServices;
            _applicationUserServices = applicationUserServices;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cartItems = await _shoppingCartServices.GetUserCartItemsAsync(userId);
            var user = await _applicationUserServices.GetUserByIdAsync(userId);

            if (user is null)
            {
                return NotFound();
            }

            ShoppingCartVM shoppingCartVM = new()
            {
                ShoppingCartList = cartItems,
                OrderHeader = new()
            };

            shoppingCartVM.OrderHeader.ApplicationUser = user;
            shoppingCartVM.OrderHeader.ApplicationUserId = userId;
            shoppingCartVM.OrderHeader.Name = user.Name;
            shoppingCartVM.OrderHeader.PhoneNumber = user.PhoneNumber;
            shoppingCartVM.OrderHeader.StreetAddress = user.StreetAddress;
            shoppingCartVM.OrderHeader.City = user.City;
            shoppingCartVM.OrderHeader.State = user.State;
            shoppingCartVM.OrderHeader.PostalCode = user.PostalCode;

            foreach (var carts in shoppingCartVM.ShoppingCartList)
            {
                shoppingCartVM.OrderHeader.OrderTotal += (carts.Price * carts.Count);
            }
            return View(shoppingCartVM);
        }
        public async Task<IActionResult> Plus(int cartId)
        {
            var cartItems = await _shoppingCartServices.GetCartByIdAsync(cartId);
            if (cartItems is null)
            {
                return NotFound();
            }
            cartItems.Count++;
            await _shoppingCartServices.UpdateCartAsync(cartItems);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Minus(int cartId)
        {
            var cartItems = await _shoppingCartServices.GetCartByIdAsync(cartId);
            if (cartItems is null || cartItems.Count <= 0)
            {
                await _shoppingCartServices.UpdateCartAsync(cartItems);
            }
            cartItems.Count--;
            await _shoppingCartServices.UpdateCartAsync(cartItems);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int cartId)
        {
            var cartItems = await _shoppingCartServices.GetCartByIdAsync(cartId);
            cartItems.Count = 0;
            await _shoppingCartServices.UpdateCartAsync(cartItems);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> UpdateCart(int cartId, int count)
        {
            var cartItems = await _shoppingCartServices.GetCartByIdAsync(cartId);
            if (cartItems is null)
            {
                return NotFound();
            }
            if (count <= 1)
            {
                cartItems.Count = 0;
                await _shoppingCartServices.UpdateCartAsync(cartItems);
            }
            else
            {
                if (count >= 1000)
                {
                    cartItems.Count = 1000;
                }
                else
                {
                    cartItems.Count = count;
                }
            }
            await _shoppingCartServices.UpdateCartAsync(cartItems);
            return Ok(new { success = true });
        }
    }
}
