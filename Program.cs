using Arihant.Models.JWT;
using Arihant.Models.Middleware;
using Arihant.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<SessionTimeoutFilter>();
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<DL_Users_Master>();
builder.Services.AddTransient<DL_Login>();
builder.Services.AddTransient<DL_Email>();
builder.Services.AddTransient<DL_BOM>();
builder.Services.AddScoped<Arihant.Services.JWT>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero 
    };


    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["X-Auth-Token"];
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {

            context.HandleResponse();
            context.Response.Redirect("/LogIn/User_Login");
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            string referer = context.Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer)) referer = "/ArihantERP/Home/Index";

            context.Response.Redirect(referer + (referer.Contains("?") ? "&" : "?") + "unauthorized=true");
            //context.Response.Redirect("/LogIn/UnAuthorized");
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicPolicyProvider>();

builder.Services.AddAuthorization();

builder.Services.AddScoped<GraficaliClasses.GraficaliClasses>(provider =>
{

    var connectionString = builder.Configuration.GetConnectionString("ConnSQLString");
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    return new GraficaliClasses.GraficaliClasses(connectionString, httpContextAccessor);
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    
});


var app = builder.Build();


//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");

//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();
//app.UseMiddleware<MenuAuthorizationMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LogIn}/{action=User_LogIn}/{id?}");

app.Run();
