using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.Models;
using MyUserWebApp.ViewModels;
using System.Security.Claims;
using NPOI.SS.Formula.Functions;
using System.Web.Mvc;

namespace MyUserWebApp.MyRepository
{
    public class CRUD_Repository : ICRUD_Repository
    {
        UserManager<MyUser> _userManager;

        public CRUD_Repository(UserManager<MyUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Delete(string id)
        {
            MyUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RankException("User does not exist");
            }
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // Успешно удалено
                return true;
            }
            else
            {
                // Ошибка при удалении пользователя
                return false;
            }
        }

        private bool RankException(string v)
        {
            throw new NotImplementedException();
        }

        public async Task<EditUserViewModel> GetViewProfile(string id)
        {
            EditUserViewModel model = new EditUserViewModel();
            MyUser user = await FindMyUserById(id);
            model.Id = user.Id;
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
                return NotFoundObjectResult("Customer is not found");
            }
            return user;
        }

        private MyUser NotFoundObjectResult(string v)
        {
            throw new NotImplementedException();
        }

        public async Task<MyUser> FindMyUserById(string id)
        {
            MyUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFoundObjectResult("Customer is not found");
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
                model.LastName = user.LastName;
                model.Email = user.Email;
                model.Image = user.Image;
            }
            catch (Exception e)
            {
                new ArgumentNullException(e.Message);
            }
            return model;
        }
        public  string Avatar(string name)
        {
            var userId = FindMyUser(name).Id;
            var imagePath = $"~/store/avatars/{userId}.png";  
            //var mimeType = "image/png";  

            return imagePath;
        }

        public async Task<IItem> Edit(IFormFile avatarFile, EditUserViewModel model)
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
                    user.Image = await UploadAvatar(avatarFile,model.Id);
                    return (IItem)user;
                }
            return (IItem)model;
        }
        // завантаження автарки користувача
        public async Task<string> UploadAvatar(IFormFile avatarFile,string id)
        {

                if (avatarFile == null || avatarFile.Length == 0)
                {
                    // Обробка помилки, якщо файл не було завантажено
                    return ArgumentNullException("The file was not loaded.");
                }

                // Створення унікального імені файлу
                string uniqueFileName = id + ".png";

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

                //TempData["Message"] = "Avatar was uploaded successfully";
                return filePath;
        }

        private string ArgumentNullException(string v)
        {
            throw new NotImplementedException();
        }
    }
}
