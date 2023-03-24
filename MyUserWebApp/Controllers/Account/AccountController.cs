using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNet.Identity;
using MyUserWebApp.ViewModels;
using MyUserWebApp.Models;

namespace MyUserWebApp.Controllers.Account
{
    public class AccountController:Controller
    {
        private readonly SignInManager<MyUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<MyUser> _userManager;
        //private readonly IUserStore<MyUser> _userStore;
        //private readonly IUserEmailStore<MyUser> _emailStore;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        public AccountController(
            Microsoft.AspNetCore.Identity.UserManager<MyUser> userManager,
            //IUserStore<MyUser> userStore,
            SignInManager<MyUser> signInManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            //_userStore = userStore;
            //_emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager= roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(IFormCollection form, RegisterModel model)
        {
          
            
            if (ModelState.IsValid)
            {
               
                MyUser user = new MyUser { Email = model.Email, UserName = model.Email};
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);

                //получаем роль
                var role = await _roleManager.FindByNameAsync(form["Role"].ToString());
                if (result.Succeeded==true && role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                   // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
       
        
        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

        
        [HttpGet]
        public IActionResult Login(string returnUrl = null)

        { 
            return View(new LoginModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!string.IsNullOrEmpty(model.Email)&& !string.IsNullOrEmpty(model.Password))
            {
                //var user =await _userManager.FindByEmailAsync(model.Email);

                var result =await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Home");
                    // проверяем, принадлежит ли URL приложению
                    //if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    //{
                    //    _logger.LogInformation("User logged in.");
                    //    return Redirect(model.ReturnUrl);

                    //}
                    //else
                    //{
                    //    return RedirectToAction("Index", "Home");
                    //}
                }
                else
                {
                    ModelState.AddModelError("", "Wrong username and/or password");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //public async Task OnGetAsync(string returnUrl = null)
        //{
        //    ReturnUrl = returnUrl;
        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        //}

        

        private MyUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<MyUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(MyUser)}'. " +
                    $"Ensure that '{nameof(MyUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        //private IUserEmailStore<MyUser> GetEmailStore()
        //{
        //    if (!_userManager.SupportsUserEmail)
        //    {
        //        throw new NotSupportedException("The default UI requires a user store with email support.");
        //    }
        //    return (IUserEmailStore<MyUser>)_userStore;
        //}
    }
}

