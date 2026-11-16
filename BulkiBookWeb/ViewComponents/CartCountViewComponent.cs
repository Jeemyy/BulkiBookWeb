using BulkiBook.Business.Interfaces;
using BulkiBook.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkiBookWeb.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly IShoppingCartServices _shoppingCartService;
        public CartCountViewComponent(IShoppingCartServices shoppingCart)
        {
            _shoppingCartService = shoppingCart;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                HttpContext.Session.Remove(SD.SessionCart);
                return View(0);
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var user = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (user is null)
            {
                HttpContext.Session.Remove(SD.SessionCart);
                return View(0);
            }

            var sessionCount = HttpContext.Session.GetInt32(SD.SessionCart);
            if (sessionCount.HasValue)
            {
                return View(sessionCount);
            }
            var count = await _shoppingCartService.GetCartCountAsync(user.Value);
            HttpContext.Session.SetInt32( SD.SessionCart, count);

            return View(count);
        }
    }
}
