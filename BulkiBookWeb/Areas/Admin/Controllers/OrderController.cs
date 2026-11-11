using BulkiBook.Business.Interfaces;
using BulkiBook.Utility;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkiBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.RoleAdmin)]
    [Authorize(Roles = SD.RoleAdmin + "," + SD.RoleEmployee + "," + SD.RoleCustomer)]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;

        [BindProperty]
        public OrderHeader OrderHeader { get; set; }

        public OrderController(IOrderServices order)
        {
            _orderServices = order;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int Id)
        {
            OrderHeader = await _orderServices.GetOrderByIdAsync(Id, includeUser: true, includeDetails: true);
            //if(OrderHeader is null)
            //{
            //    return NotFound();
            //}
            return View(OrderHeader);
        }
        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin + "," + SD.RoleEmployee)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder()
        {
            if (OrderHeader is null)
            {
                TempData["error"] = "Order not found";
                return RedirectToAction(nameof(Index));
            }

            var orderHeaderFromDb = await _orderServices.GetOrderByIdAsync(OrderHeader.Id);
            if (orderHeaderFromDb is null)
            {
                return NotFound();
            }

            orderHeaderFromDb.Name = OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderHeader.City;
            orderHeaderFromDb.State = OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderHeader.PostalCode;

            if (orderHeaderFromDb.OrderStatus != SD.StatusPending &&
        orderHeaderFromDb.OrderStatus != SD.StatusApproved)
            {
                orderHeaderFromDb.Carrier = OrderHeader.Carrier;
                orderHeaderFromDb.TrackingNumber = OrderHeader.TrackingNumber;
            }


            await _orderServices.UpdateOrderAsync(orderHeaderFromDb);
            TempData["success"] = "Update order is successfully.";
            return RedirectToAction(nameof(Details), new { id = orderHeaderFromDb.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin + "," + SD.RoleEmployee)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(string status)
        {
            if (OrderHeader is null)
            {
                TempData["error"] = "Order not found";
                return BadRequest();
            }

            var orderHeaderFromDb = await _orderServices.GetOrderByIdAsync(OrderHeader.Id);

            if(orderHeaderFromDb is null)
            {
                TempData["error"] = "Order not found";
                return RedirectToAction(nameof(Index));
            }

            string successMassage;
            switch (status)
            {
                case SD.StatusInProcess:
                    await _orderServices.UpdateOrderStatusAsync(OrderHeader.Id, status);
                    successMassage = "Order processing started successfully";
                    break;
                case SD.StatusCancelled:
                    await _orderServices.UpdateOrderStatusAsync(OrderHeader.Id, status);
                    successMassage = "Order cancelled successfully";
                    return RedirectToAction(nameof(Index), new { id = OrderHeader.Id });

                case SD.StatusRefunded:
                    await _orderServices.UpdateOrderStatusAsync(OrderHeader.Id, status);
                    successMassage = "Order refunded successfully";
                    return RedirectToAction(nameof(Index), new { id = OrderHeader.Id });


                case SD.StatusShipped:
                    if (string.IsNullOrEmpty(OrderHeader.Carrier) || string.IsNullOrEmpty(OrderHeader.TrackingNumber))
                    {
                        TempData["error"] = "Please provide both carrier and tracking number!";
                        return RedirectToAction(nameof(Details), new { id = OrderHeader.Id });
                    }
                    await _orderServices.UpdateOrderStatusAsync(OrderHeader.Id, status, OrderHeader.Carrier, OrderHeader.TrackingNumber);
                    successMassage = "Order shipped successfully";
                    break;

                default:
                    TempData["error"] = "Invalid status update!";
                    return RedirectToAction(nameof(Details), new { id = OrderHeader.Id });
            }

            TempData["success"] = successMassage;
            return RedirectToAction(nameof(Details), new { id = OrderHeader.Id });
        }
        [ActionName("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            await _orderServices.RemoveOrderAsync(id);
            return RedirectToAction(nameof(Index), new { id = id });
        }
        //#region API CALLS
        //[AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll(string? status)
        {
            string? userId = null;
            if (!User.IsInRole(SD.RoleAdmin) && !User.IsInRole(SD.RoleEmployee))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }
            }
            var orders = await _orderServices.GetAllOrderAsync(userId, status, includeUser: true);
            return Json(new { data = orders });
        }
    }
}