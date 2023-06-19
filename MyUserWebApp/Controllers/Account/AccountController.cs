using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using MyUserWebApp.ViewModels;
using MyUserWebApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MyUserWebApp.MyRepository;
using MyUserWebApp.Services;

namespace MyUserWebApp.Controllers.Account
{
    public class AccountController:Controller
    {
        private readonly SignInManager<MyUser> _signInManager;
        //private readonly UserManager<MyUser> _userManager;
        //private readonly IUserStore<MyUser> _userStore;
        //private readonly IUserEmailStore<MyUser> _emailStore;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CRUD_Repository _cRUD;
        private readonly UserService _userService;
        public AccountController(
            //UserManager<MyUser> userManager,
            //IUserStore<MyUser> userStore,
            SignInManager<MyUser> signInManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender, /*RoleManager<IdentityRole> roleManager,*/
            /*CRUD_Repository cRUD,*/ UserService userService)

        {
            //_userManager = userManager;
            //_userStore = userStore;
            //_emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            //_roleManager= roleManager;
            //_cRUD = cRUD;
            //_roleManager= roleManager;
            _userService= userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(IFormCollection form, RegisterModel model)
        {
            
            if (ModelState.IsValid && await _userService.CreateUser(form, model) == true)
            { 
                return RedirectToAction("Index", "Home"); 
            } 
                return View(model); 
        }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        [TempData]
          public string ErrorMessage { get; set; }
        
        [HttpGet]
        public async Task<IActionResult> LoginAsync(string returnUrl=null)
        {           
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return View(new LoginModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.Login(model)==true)
                {                   
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            ViewBag.UserId = userId;
                            ViewBag.UserEmail = model.Email;
                            
                        }
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return View(model);
                    }
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
            if(await _userService.LogOut()==true)
            return RedirectToAction("Index", "Home");
            return RedirectToAction("Error", "Home");
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

