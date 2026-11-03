using BulkiBook.Business.Interfaces;
using BulkiBook.DataAccess.Data;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkiBook.Business.Services
{
    public class ShoppingCartServices : IShoppingCartServices
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ShoppingCart> AddToCartAsync(ShoppingCart cart)
        {
            var cartItems = await _context.ShoppingCarts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == cart.ApplicationUserId && c.ProductId == cart.ProductId);
            if (cartItems is not null)
            {
                cartItems.Count += cart.Count;
                await _context.SaveChangesAsync();
                return cartItems;
            }
            else
            {

                _context.ShoppingCarts.Add(cart);
                cart.Id = 0;
                await _context.SaveChangesAsync();
                return cart;
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cartItems = await _context.ShoppingCarts
                .Include(c => c.Product)
                .Where(c => c.ApplicationUserId == userId)
                .ToListAsync();
            if (cartItems.Any())
            {
                _context.ShoppingCarts.RemoveRange(cartItems);
                _context.SaveChangesAsync();
            }
        }

        public async Task<ShoppingCart?> GetCartByIdAsync(int cartId)
        {
            return await _context.ShoppingCarts.Include(c => c.Product).FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public Task<int> GetCartCountAsync(string userId)
        {
            return _context.ShoppingCarts.Where(c => c.ApplicationUserId == userId).SumAsync(c => c.Count);
        }

        public async Task<IEnumerable<ShoppingCart>> GetUserCartItemsAsync(string userId)
        {
            return await _context.ShoppingCarts.Include(c => c.Product).Where(c => c.ApplicationUserId == userId).ToListAsync();
        }

        public async Task UpdateCartAsync(ShoppingCart cart)
        {
            if (cart.Count <= 0)
            {
                _context.ShoppingCarts.Remove(cart);
            }
            else
            {
                _context.ShoppingCarts.Update(cart);
            }
            await _context.SaveChangesAsync();
        }
    }
}
