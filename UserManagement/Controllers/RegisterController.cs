using Microsoft.AspNetCore.Mvc;
using UserManagement.DAL;
using UserManagement.Interface;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IDALRgister _reg;

        // Use constructor injection to get the instance of IDALRgister
        public RegisterController(IDALRgister reg)
        {
            _reg = reg;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult RegisterUser(RegisterModel reg)
        {
            ResponseModel result = new ResponseModel();
            result = _reg.Register(reg);
            return Json(result);
        }
    }
}
