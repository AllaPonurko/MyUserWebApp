using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.Models;
using MyUserWebApp.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace MyUserWebApp.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        UserManager<MyUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<MyUser> userManager)
        {
            _logger = logger;
            _userManager=userManager;
        }
        
        public IActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    ViewBag.UserName = User.Identity.Name;
            //}
            return View();           
        }
       
        public IActionResult UserPage()
        {
            if (User.Identity.IsAuthenticated)
                return Content(User.Identity.Name);
            else
                return Content("You are not logged in ");
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
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            EditUserViewModel model = new EditUserViewModel();
            if (User.Identity.IsAuthenticated)
            {
                model.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.UserId = model.Id;
            }
            MyUser user = await _userManager.FindByIdAsync(model.Id);
                 if (user == null)
                 {
                    return NotFound();
                 }
            model.Email = user.Email;
            model.LastName = user.LastName;
            model.FirstName = user.FirstName;
                
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                MyUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Id = model.Id;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    var result = await _userManager.UpdateAsync(user);
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
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            MyUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.IdentityResult result =
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
    }
}