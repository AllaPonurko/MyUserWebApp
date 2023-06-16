using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.Models;
using MyUserWebApp.MyRepository;
using MyUserWebApp.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace MyUserWebApp.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CRUD_Repository _cRUD;
        public HomeController(ILogger<HomeController> logger,CRUD_Repository cRUD)
        {
            _logger = logger;
            _cRUD= cRUD;
        }
        
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserName = User.Identity.Name;
                ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                MyUser user = await _cRUD.FindMyUser(User.Identity.Name);
                string? avatarPath = user.Image;
                ViewBag.AvatarPath = avatarPath;
            }
            return View();           
        }
       
        
        public IActionResult Shoping()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}