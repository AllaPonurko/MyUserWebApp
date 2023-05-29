using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.Models;
using MyUserWebApp.ViewModels;
using System.Security.Claims;
using NPOI.SS.Formula.Functions;

namespace MyUserWebApp.MyRepository
{
    public class RepositoryCRUD : Controller, IRepositoryDelete, IRepositoryUpdate, IRepositoryRead
    {
        UserManager<MyUser> _userManager;

        public RepositoryCRUD(UserManager<MyUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ActionResult> Delete(string id)
        {
            EditUserViewModel model = new EditUserViewModel();
            if (User.Identity.IsAuthenticated)
            {
                model.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            MyUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
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
        public async Task<EditUserViewModel> GetViewProfile(string id)
        {
            
            EditUserViewModel model = new EditUserViewModel();
            MyUser user = await FindMyUserById(id);
            model.Id = user.Id;
            ViewBag.UserId = model.Id;
            //ViewData["UserId"] = model.Id;

            model.Email = user.Email;
            model.LastName = user.LastName;
            model.FirstName = user.FirstName;
            model.AboutMe = user.AboutMe;
            model.Image = user.Image;
            return model;
        }


        public async Task<MyUser> FindMyUser(string name)
        {
            MyUser user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return null;
            }
            return user;
        }
        public async Task<MyUser> FindMyUserById(string id)
        {
            MyUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<IItem> Profile(string userName)
        {
            ProfileViewModel model = new ProfileViewModel();
            MyUser user = await FindMyUser(userName);
            try
            {
                model.Id = user.Id;
                model.FirstName = user.FirstName;
                model.Email = user.Email;
                model.Image = user.Image;
                ViewBag.UserId = user.Id;

            }
            catch (Exception e)
            {
                new ArgumentNullException(e.Message);
            }
            return model;
        }
        public async Task<IItem> Edit(IFormFile avatarFile, EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyUser user = await FindMyUserById(model.Id);
                if (user != null)
                {
                    user.Id = model.Id;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.AboutMe = model.AboutMe;
                    user.Image = await UploadAvatar(avatarFile);

                    TempData["SuccessMessage"] = "The image has been uploaded successfully";

                    //var result = await _userManager.UpdateAsync(user);
                    return (IItem)user;
                }
            }
            return (IItem)model;
        }
        // завантаження автарки користувача
        public async Task<string> UploadAvatar(IFormFile avatarFile)
        {

                if (avatarFile == null || avatarFile.Length == 0)
                {

                    // Обробка помилки, якщо файл не було завантажено
                    return null;
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

                // Додаткова логіка для збереження шляху до файлу в базі даних або іншому місці

                TempData["Message"] = "Avatar was uploaded successfully";
                return filePath;
        }
        
    }
}
