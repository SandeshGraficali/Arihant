using Arihant.Models.Menu;
using Arihant.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace Arihant.Controllers
{
    public class LogInController : Controller
    {
        
        private readonly DL_Login _user;
        private readonly DL_Email _email;
        private readonly JWT _jwt;
        public LogInController(DL_Login _master , DL_Email email  , JWT token)
        {
            _user = _master;
            _email = email;
            _jwt = token;
        }


      


        public IActionResult User_LogIn()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SendOTP(string email)
        {
            try
            {
                string otp = new Random().Next(100000, 999999).ToString();
                string bodyContent = $"Your OTP for Login: <b>{otp}</b>";
                bool isSent = _email.SendEmail(email , bodyContent,"OTP for Login" );
                               _email.SaveOTP(email, otp);

                if (isSent)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Email not found or error sending mail." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult UpdatePassword(string email, string password)
        {
            try
            {
                int rowsAffected = _email.UpdateAccountPassword(email, password);

                if (rowsAffected > 0)
                {
                    return Json(new { success = true, message = "Password updated successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Update failed. User not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult VerifyOTP(string email, string otp)
        {
            try
            {
               
                    string status = _email.VerifyOTP(email, otp);

                if (status == "Success")
                {
                    DataSet ds = _email.SetSession(email);
                    if (ds.Tables[0].Rows.Count>0 )
                    {
                        string Username =ds.Tables[0].Rows[0]["UserName"].ToString();
                        string ID = ds.Tables[0].Rows[0]["ID"].ToString();
                        HttpContext.Session.SetString("UserName", Username);
                        HttpContext.Session.SetString("EmailID", email);
                        HttpContext.Session.SetString("UserID", email);
                        List<MenuViewModel> menus = _user.GetUserAccess(email);
                        string menuJson = JsonConvert.SerializeObject(menus);

                        string token = _jwt.GenerateJwtToken(email , email , menus);
                        //Response.Cookies.Append("X-Auth-Token", token, new CookieOptions
                        //{
                        //    HttpOnly = true,  
                        //    Secure = true,     
                        //    SameSite = SameSiteMode.Strict,
                        //    Expires = DateTimeOffset.UtcNow.AddHours(8)
                        //});
                        HttpContext.Session.SetString("JWT_Token", token);
                        HttpContext.Session.SetString("UserMenu", menuJson);
                    }
                    else
                    {

                    }

                        
                    return Json(new { success = true, message = "OTP Verified successfully." });
                }
                else if (status == "Expired")
                {
                    return Json(new { success = false, message = "Invalid OTP." });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid OTP. Please try again." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Login(string Username, string Password)
        {
           
            string result= _user.CheckUserIsValid(Username, Password);
          
            if (result == "1")
            {
                string userIP = HttpContext.Connection.RemoteIpAddress?.ToString();
                string IPSAcces = _user.CheckIP(Username, userIP);
                if (IPSAcces == "1")
                {

                }
                else
                {
                    return Json(new { success = false, message = "Your IP address is not authorized." });

                }

                string otp = new Random().Next(100000, 999999).ToString();
                string bodyContent = $"Your OTP for Login: <b>{otp}</b>";
                bool isSent = _email.SendEmail(Username, bodyContent, "OTP for Login");
                _email.SaveOTP(Username, otp);
               
                return Json(new { success = true, message = "Login successful!" });
            }
            else if (result == "3")
            {
                
                return Json(new { success = false, message = "Your password has expired. Please reset it." });
            }
            else
            {
                return Json(new { success = false, message = "Invalid username or password." });
            }

        }

        public IActionResult UnAuthorized()
        {
            return View(); 
        }

        public ActionResult Logout()
        {
            Response.Cookies.Delete("X-Auth-Token");
            HttpContext.Session.Clear();
            return RedirectToAction("User_LogIN", "LogIn");
        }

    }
}
