using BulkiBook.Utility;
using BulkyBook.Models;
using BulkyBook.Models.ModelViews;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace BulkiBookWeb.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<ApplicationUser> user, SignInManager<ApplicationUser> sign, RoleManager<IdentityRole> role)
        {
            _userManager    = user;
            _signInManager  = sign;
            _roleManager    = role;
        }
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password,
                    login.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                }
            }
            return View(login);
        }
        public IActionResult Register(string? returnUrl = null)
        {
            var model = new RegisterVM
            {
                RoleList = new List<SelectListItem>
                {
                    new SelectListItem{Text = SD.RoleCustomer, Value = SD.RoleCustomer},
                    new SelectListItem{Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                    new SelectListItem{Text = SD.RoleEmployee, Value = SD.RoleEmployee}
                }
            };
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register, string? returnUrl = null)
        {
            if (!await _roleManager.RoleExistsAsync(SD.RoleCustomer))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.RoleCustomer));
                await _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin));
                await _roleManager.CreateAsync(new IdentityRole(SD.RoleEmployee));
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName        = register.Email,
                    Email           = register.Email,
                    Name            = register.Name,
                    PhoneNumber     = register.PhoneNumber,
                    StreetAddress   = register.StreetAddress,
                    City            = register.City,
                    State           = register.State,
                    PostalCode      = register.PostalCode
                };
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(register.Role))
                    {
                        await _userManager.AddToRoleAsync(user, register.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.RoleCustomer);
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home", new {area = "Customer"});
                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
            }
            return View(register);
        }
        public IActionResult AccessDeny()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}
