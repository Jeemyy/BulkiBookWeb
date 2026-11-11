using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Migrations;
using BulkiBook.Utility;
using BulkyBook.Models;
using BulkyBook.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkiBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IShoppingCartServices _shoppingCartServices;
        private readonly IApplicationUserServices _applicationUserServices;
        private readonly IOrderServices _orderServices;
        public CartController(IProductServices product, IShoppingCartServices shoppingCartServices, IApplicationUserServices applicationUserServices, IOrderServices orderServices)
        {
            _shoppingCartServices = shoppingCartServices;
            _applicationUserServices = applicationUserServices;
            _orderServices = orderServices;
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

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPOST(ShoppingCartVM shoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cartItems = await _shoppingCartServices.GetUserCartItemsAsync(userId);

            shoppingCartVM.ShoppingCartList = cartItems;
            shoppingCartVM.OrderHeader.OrderDate = DateTime.UtcNow;
            shoppingCartVM.OrderHeader.ApplicationUserId = userId;


            foreach (var carts in shoppingCartVM.ShoppingCartList)
            {
                shoppingCartVM.OrderHeader.OrderTotal += (carts.Price * carts.Count);
            }

            shoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            shoppingCartVM.OrderHeader.OrderDetails = shoppingCartVM.ShoppingCartList
                .Select(c => new OrderDetails
                {
                    ProductId = c.ProductId,
                    Count = c.Count,
                    Price = c.Price
                }).ToList();

            await _orderServices.CreateOrderAsync(shoppingCartVM.OrderHeader);


            return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartVM.OrderHeader.Id });
        }
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            return View(id);
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
