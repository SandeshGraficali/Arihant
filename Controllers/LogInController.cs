using Arihant.Services;
using Microsoft.AspNetCore.Mvc;

namespace Arihant.Controllers
{
    public class LogInController : Controller
    {
        private readonly DL_Login _user;
        public LogInController(DL_Login _master)
        {
            _user = _master;
        }


        public IActionResult User_LogIn()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string Username, string Password)
        {
           
            string result= _user.CheckUserIsValid(Username, Password);
            if (result == "1")
            {
                HttpContext.Session.SetString("UserName", Username);
                return Json(new { success = true, message = "Login successful!" });
            }
            else
                return Json(new { success = false, message = "Invalid username or password." });

        }

    }
}
