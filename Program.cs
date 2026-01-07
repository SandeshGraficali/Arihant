using Arihant.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<DL_Users_Master>();
builder.Services.AddTransient<DL_Login>();
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


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users_Master}/{action=User_Master_Dashboard}/{id?}");

app.Run();
