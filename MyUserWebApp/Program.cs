using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyUserWebApp.Areas.Identity.Data;
using MyUserWebApp.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MyUserWebAppContextConnection") ?? throw new InvalidOperationException("Connection string 'MyUserWebAppContextConnection' not found.");

builder.Services.AddDbContext<MyUserWebAppContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<MyUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MyUserWebAppContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
