using Microsoft.AspNetCore.Mvc;
using UserManagement.Interface;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly IDALLogin _reg;

       
        public LoginController(IDALLogin reg)
        {
            _reg = reg;
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult LoginUser(RegisterModel log)
        {
            ResponseModel result = new ResponseModel();
            result = _reg.loginUser(log);
            if (result.status && result.UserID != 0)
            {
                
                HttpContext.Session.SetInt32("UserID", result.UserID);
                HttpContext.Session.SetString("Name", result.Name);
                HttpContext.Session.SetString("Email", result.Email);
                HttpContext.Session.SetString("MobileNumber", result.mobile);
                HttpContext.Session.SetString("Address", result.Address);

                result.message = "Login successful.";
            }
            else
            {
                result.message = "Invalid email or password.";
            }

            return Json(result);
        }

    }
}
