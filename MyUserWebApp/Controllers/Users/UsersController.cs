using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.Controllers.Home;
using MyUserWebApp.Models;
using MyUserWebApp.MyRepository;
using MyUserWebApp.ViewModels;
using System.Security.Claims;

namespace MyUserWebApp.Controllers.Users
{
    public class UsersController : Controller
    {
        private readonly SignInManager<MyUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        UserManager<MyUser> _userManager;
        RepositoryCRUD _cRUD;
        public UsersController(ILogger<HomeController> logger, UserManager<MyUser> userManager,
            RepositoryCRUD cRUD, SignInManager<MyUser> signInManager/*,IWebHostEnvironment hostingEnvironment*/)
        {
            _logger = logger;
            _userManager = userManager;
            _cRUD = cRUD;
            _signInManager = signInManager;
            //_hostingEnvironment=hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            if (User.Identity.IsAuthenticated)
                return Content(User.Identity.Name);
            else
                return Content("You are not logged in ");
        }
        //отримання форми с даними для редагування профілю користувача
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
            model.AboutMe = user.AboutMe;
            model.Image = user.Image;
            return View(model);
        }
        // редагування профілю користувача
        [HttpPost]
        public async Task<IActionResult> EditProfile(IFormFile avatarFile, EditUserViewModel model)
        {
            //_cRUD.Edit(model);  
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
                    user.AboutMe = model.AboutMe;

                    if (avatarFile == null || avatarFile.Length == 0)
                    {
                        // Обробка помилки, якщо файл не було завантажено
                        return BadRequest();
                    }

                    // Створення унікального імені файлу
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + avatarFile.FileName;

                    // Визначення шляху для збереження файлу
                    string urlPath = "/store/avatars";
                    string uploadsFolder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot" + urlPath));
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    //if (!Directory.Exists(filePath))
                    //{
                    //    Directory.CreateDirectory(filePath);
                    //}
                    // Збереження файлу на сервері
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(fileStream);
                    }


                    TempData["SuccessMessage"] = "The image has been uploaded successfully";

                    user.Image = filePath;
                    var result = await _userManager.UpdateAsync(user);
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
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string userId)
        {
            MyUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            // Показать диалог с подтверждением удаления
            return PartialView("_ConfirmDeletePartial");
            //await _cRUD.Delete(id);
            // return View();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                // Успешно удалено
                // Дополнительная логика или перенаправление на другую страницу
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Ошибка при удалении пользователя
                // Обработка ошибки или перенаправление на другую страницу
                return RedirectToAction("Error", "Home");
            }
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
        // завантаження автарки користувача
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatar(IFormFile avatarFile, string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            if (avatarFile == null || avatarFile.Length == 0)
            {
                // Обробка помилки, якщо файл не було завантажено
                return BadRequest();
            }

            // Створення унікального імені файлу
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + avatarFile.FileName;

            // Визначення шляху для збереження файлу
            string urlPath = "/store/avatars/";
            string uploadsFolder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot" + urlPath));
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            // Збереження файлу на сервері
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(fileStream);
            }

            // Додаткова логіка для збереження шляху до файлу в базі даних або іншому місці

            return Content("The file has been successfully uploaded ");
        }
    }
}
