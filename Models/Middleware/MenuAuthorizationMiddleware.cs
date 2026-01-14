using Arihant.Models.Menu;
using Newtonsoft.Json;

namespace Arihant.Models.Middleware
{
    public class MenuAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public MenuAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value;
            if (path.Contains("/lib") || path.Contains("/css") || path.Contains("/images") || path.Contains("/js") ||
                path == "/" || path.Contains("/LogIn") ||  path.Contains("Users_Master/Dashboard") || path.Contains("/UnAuthorized"))
            {
                await _next(context);
                return;
            }

            var menuData = context.Session.GetString("UserMenu");

            if (string.IsNullOrEmpty(menuData))
            {

                context.Response.Redirect("/ArihantERP/LogIn/User_LogIn");
                return;
                
            }

            List<MenuViewModel> menus = JsonConvert.DeserializeObject<List<MenuViewModel>>(menuData);
            bool isAuthorized = menus.Any(m =>
                !string.IsNullOrEmpty(m.UrlPage) &&
                (path.Equals(m.UrlPage, StringComparison.OrdinalIgnoreCase) ||
                 m.UrlPage.EndsWith(path, StringComparison.OrdinalIgnoreCase)));

            if (isAuthorized)
            {
                //context.Session.SetString("LastAuthorizedUrl", path);
                await _next(context);
            }
            else
            {
                context.Response.Redirect("/ArihantERP/LogIn/UnAuthorized");
            }
        }
    }
}
