using Arihant.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Arihant.Controllers
{
    public class LogInController : Controller
    {
        
        private readonly DL_Login _user;
        private readonly DL_Email _email;
        public LogInController(DL_Login _master , DL_Email email)
        {
            _user = _master;
            _email = email;
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
                string bodyContent = $"Your OTP for password reset is: <b>{otp}</b>";
                bool isSent = _email.SendEmail(email , bodyContent );
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
                    string Sesssion = _email.SetSession(email);
                    HttpContext.Session.SetString("UserName", Sesssion);
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
                string otp = new Random().Next(100000, 999999).ToString();
                string bodyContent = $"Your OTP for password reset is: <b>{otp}</b>";
                bool isSent = _email.SendEmail(Username, bodyContent);
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

    }
}
