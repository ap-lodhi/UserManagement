using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Index", "Login"); 
        }
    }
}
