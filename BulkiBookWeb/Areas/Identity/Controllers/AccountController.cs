using Microsoft.AspNetCore.Mvc;
namespace BulkiBookWeb.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult AccessDeny()
        {
            return View();
        }
    }
}
