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
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password,
                    login.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                }
            }
            return View(login);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
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
                    await _signInManager.SignInAsync(user, isPersistent: false);
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
