using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using UserManagement.Interface;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class DashbordController : Controller
    {
        private readonly IDALDashbordcs _reg;


        public DashbordController(IDALDashbordcs reg)
        {
            _reg = reg;
        }
        public IActionResult Index(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                var name = HttpContext.Session.GetString("Name");
                var email = HttpContext.Session.GetString("Email");
                var mobile = HttpContext.Session.GetString("MobileNumber");
                var address = HttpContext.Session.GetString("Address");
                
                ViewBag.UserID = userId;
                ViewBag.Name = name;
                ViewBag.Email = email;
                ViewBag.MobileNumber = mobile;
                ViewBag.Address = address;
                var data = _reg.GetUserDetails(userId.Value);
                return View(data);
            }
           
        }


         public IActionResult Update()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            

           
            var data = _reg.GetUserDetails(userId.Value);
            return View(data);
        }

        public JsonResult UpdateUser(RegisterModel obj)
        {
            ResponseModel result = new ResponseModel();
            result = _reg.UpdateUser(obj);
            return Json(result);
        }
    }
}
