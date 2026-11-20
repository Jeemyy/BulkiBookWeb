using BulkiBook.DataAccess.Data;
using BulkiBook.Utility;
using BulkyBook.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkiBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var totalOrders = await _db.OrderHeaders.ToListAsync();
            var totalProducts = await _db.Products.CountAsync();
            var totalUsers = await _db.ApplicationUsers.CountAsync();
            var totalRevenu = totalOrders
                                    .Where(o => o.OrderStatus == SD.StatusApproved || o.OrderStatus == SD.StatusShipped)
                                    .Sum(o => o.OrderTotal);
            DashboardVM dashboard = new()
            {
                TotalOrders = totalOrders.Count(),
                TotalProducts = totalProducts,
                TotalUsers = totalUsers,
                TotalRevenu = totalRevenu
            };
            return View(dashboard);
        }



        [HttpGet]
        public async Task<IActionResult> GetChartData()
        {
            var orders = await _db.OrderHeaders.ToListAsync();
            var products = await _db.Products.ToListAsync();
            var categories = await _db.Categories.ToListAsync();

            // Get the last 6 calendar months, including the current month
            var currentMonth = DateTime.UtcNow;
            var firstMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1)
                .AddMonths(-5);

            // Monthly Revenue
            var monthlyRevenue = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = firstMonth.AddMonths(i);

                    var revenue = orders
                        .Where(o =>
                            o.OrderDate.Year == month.Year &&
                            o.OrderDate.Month == month.Month &&
                            (o.OrderStatus == SD.StatusApproved ||
                             o.OrderStatus == SD.StatusShipped))
                        .Sum(o => o.OrderTotal);

                    return new
                    {
                        Label = month.ToString("MMM yyyy"),
                        Revenue = revenue
                    };
                }).ToList();

            // Monthly Orders Count
            var monthlyOrders = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = firstMonth.AddMonths(i);

                    var count = orders.Count(o =>
                        o.OrderDate.Year == month.Year &&
                        o.OrderDate.Month == month.Month);

                    return new
                    {
                        Label = month.ToString("MMM yyyy"),
                        Count = count
                    };
                }).ToList();

            // Order Status Breakdown
            var orderStatusBreakdown = orders
                .GroupBy(o => o.OrderStatus ?? "Unknown")
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                }).ToList();

            // Products Count By Category
            var productsPerCategory = categories
     .Select(c => new
     {
         Category = c.Name,
         Count = products.Count(p => p.CategoryId == c.Id)
     }).ToList();

            return Json(new
            {
                monthlyRevenue,
                monthlyOrders,
                orderStatusBreakdown,
                productsPerCategory
            });
        }
    }
}
