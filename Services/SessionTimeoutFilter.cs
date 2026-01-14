using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Arihant.Services
{
    public class SessionTimeoutFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.RouteData.Values["controller"]?.ToString();


            if (controllerName == "LogIn")
            {
                return;
            }

            var sessionUser = context.HttpContext.Session.GetString("UserID");
            var authCookie = context.HttpContext.Request.Cookies["X-Auth-Token"];

            if (string.IsNullOrEmpty(authCookie) || string.IsNullOrEmpty(sessionUser))
            {
                context.HttpContext.Response.Cookies.Delete("X-Auth-Token");
                if (IsAjaxRequest(context.HttpContext.Request))
                {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    context.Result = new RedirectToActionResult("User_Login", "LogIn", new { sessionExpired = "true" });
                }
            }
        }
        private bool IsAjaxRequest(HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                   request.Headers["Accept"].ToString().Contains("application/json");
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
