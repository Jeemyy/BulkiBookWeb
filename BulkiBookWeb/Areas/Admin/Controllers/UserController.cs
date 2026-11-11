using Azure.Core.Serialization;
using BulkiBook.Business.Interfaces;
using BulkiBook.Utility;
using BulkyBook.Models;
using BulkyBook.Models.ModelViews;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BulkiBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IApplicationUserServices _applicationUserServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IApplicationUserServices applicationUserServices, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _applicationUserServices = applicationUserServices;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> RoleManagement(string userId)
        {
            var user = await _applicationUserServices.GetUserByIdAsync(userId);
            if (user is null)
            {
                return Json(new { success = false, message = "User not found!" });
            }

            RoleManagementVM roleVm = new()
            {
                ApplicationUser = user,
                RoleList = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
            };
            roleVm.ApplicationUser.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            return View(roleVm);
        }

        [HttpPost]
        [ActionName("RoleManagement")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleManagement(RoleManagementVM roleVm)
        {
            var user = await _applicationUserServices.GetUserByIdAsync(roleVm.ApplicationUser.Id);
            if(user is null)
            {
                return Json(new {success = false, message = "User not found!"});
            }

            string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            //string oldRole = SD.RoleEmployee;

            if (!(roleVm.ApplicationUser.Role == oldRole))
            {
                await _userManager.RemoveFromRoleAsync(user, oldRole);
                await _userManager.AddToRoleAsync(user, roleVm.ApplicationUser.Role);
            }
            TempData["success"] = "Update role successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangePassword(string userId)
        {
            var user = await _applicationUserServices.GetUserByIdAsync(userId);
            if (user is null)
            {
                return Json(new { success = false, message = "User not found!" });
            }
            AdminChangePasswordVM adminVm = new()
            {
                UserEmail = user.Email,
                UserName = user.Name
            };

            return View(adminVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(AdminChangePasswordVM adminVm)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(adminVm);
            //}

            var user = await _applicationUserServices.GetUserByIdAsync(adminVm.UserId);
            if(user is null)
            {
                return NotFound();
            }

            var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, passwordToken, adminVm.NewPassword);

            if (result.Succeeded)
            {
                TempData["success"] = $"Password for {user.Email} has been changed successfully";
                return RedirectToAction(nameof(Index));
            }
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("",err.Description);
            }
            adminVm.UserEmail = user.Email;
            return View(adminVm);
        }

        #region Api EndPoints
        public async Task<IActionResult> GetAll()
        {
            var users = await _applicationUserServices.GetAllUserAsync();
            foreach (var user in users)
            {
                user.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            }
            return Json(new { data = users });
        }

        [HttpPost]
        public async Task<IActionResult> LockUnLock([FromBody] string userId)
        {
            var user = await _applicationUserServices.GetUserByIdAsync(userId);
            if (user is null)
            {
                return Json(new { success = false, message = "User not found!" });
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                return Json(new { success = true, message = "User unlocked successfully" });
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(10));
                return Json(new {success = true, message = "User locked succesfully"});
            }
        }
        #endregion
    }
}
