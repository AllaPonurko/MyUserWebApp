using Azure.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.Controllers.Home;
using MyUserWebApp.Models;
using MyUserWebApp.MyRepository;
using MyUserWebApp.Services;
using MyUserWebApp.ViewModels;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace MyUserWebApp.Controllers.Users
{
    public class UsersController : Controller
    {
        private readonly SignInManager<MyUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<MyUser> _userManager;
        private readonly CRUD_Repository _cRUD;
        private readonly UserService _userService;
        public UsersController(ILogger<HomeController> logger, UserManager<MyUser> userManager,
            CRUD_Repository cRUD, SignInManager<MyUser> signInManager, UserService userService/*,IWebHostEnvironment hostingEnvironment*/)
        {
            _logger = logger;
            _userManager = userManager;
            _cRUD = cRUD;
            _signInManager = signInManager;
            _userService= userService;
            //_hostingEnvironment=hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        //отримання форми с даними для редагування профілю користувача
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserName = User.Identity.Name;
                ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                EditUserViewModel model = await _cRUD.GetViewProfile(id);
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
            
            
        }
        // редагування профілю користувача
        [HttpPost]
        public async Task<IActionResult> EditProfile(IFormFile avatarFile, EditUserViewModel model)
        {
            var user= await _userService.UpdateUser(avatarFile, model);
            var result = await _userManager.UpdateAsync((MyUser)user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                   ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string userId)
        {
            if (await _userService.DeleteUser(userId) == true)

                return RedirectToAction("Index", "Home");

            return RedirectToAction("Error", "Home");
            //else
            //{
            //    // Ошибка при удалении пользователя
            //    // Обработка ошибки или перенаправление на другую страницу
            //    return RedirectToAction("Error", "Home");
            //}
        }
        
        public async Task<IActionResult> ChangePassword(string id)
        {
            MyUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }
        // зміна паролю користувачем
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User is not found");
                }
            }
            return View(model);

        }
       
        
        
        [HttpGet]
        public async Task<IActionResult>GetProfile(string name)
        {
            ViewBag.UserName = User.Identity.Name;
            ProfileViewModel model = (ProfileViewModel)await _cRUD.Profile(name);
            return View(model);
        }
    }
}
