using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.Models;
using MyUserWebApp.ViewModels;
using System.Security.Claims;
using NPOI.SS.Formula.Functions;
using System.Web.Mvc;
using MyUserWebApp.MyException;
using System.Collections;

namespace MyUserWebApp.MyRepository
{
    public class CRUD_Repository : ICRUD_Repository
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AllException _exciption;
        private readonly SignInManager<MyUser> _signInManager;
        public CRUD_Repository(UserManager<MyUser> userManager, AllException exciption,
            RoleManager<IdentityRole> roleManager, SignInManager<MyUser> signInManager)
        {
            _userManager = userManager;
            _exciption = exciption;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public Task<ICollection>? GetAllUsers()
        {
            return _userManager.Users as ICollection as Task<ICollection>;
        }
        //видалення користувача
        public async Task<bool> Delete(string userId)
        {
            MyUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return _exciption.RankException("User does not exist");
            }
            await _signInManager.SignOutAsync();
            var result = await _userManager.DeleteAsync(user);
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

        //пошук користувача по name
        public async Task<MyUser> FindMyUser(string name)
        {
            MyUser user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return _exciption.NotFoundObjectResult("Customer is not found");
            }
            return user;
        }


        //пошук користувача по id
        public async Task<MyUser> FindMyUserById(string id)
        {
            MyUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return _exciption.NotFoundObjectResult("Customer is not found");
            }
            return user;
        }
        //отримання моделі профілю користувача
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
        //аватар користувача
        public  string Avatar(string name)
        {
            var userId = FindMyUser(name).Id;
            var imagePath = $"~/store/avatars/{userId}.png";  
            //var mimeType = "image/png";  

            return imagePath;
        }
        //редагування профілю користувача
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
                    return _exciption.ArgumentNullException("The file was not loaded.");
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
        
        //реєстрація користувача
        public async Task<bool> Register(IFormCollection form, RegisterModel model)
        {
           
                string[] l = form["Role"].ToString().Split(",");
                MyUser user = new MyUser { Email = model.Email, UserName = model.Email };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);

                //получаем роль
                List<object> roles = new List<object>();
                foreach (string role in l)
                {
                    roles.Add(await _roleManager.FindByNameAsync(role));
                }

                if (result.Succeeded == true && roles.Capacity != 0)
                {
                    foreach (var r in roles)
                    {
                        await _userManager.AddToRoleAsync(user, r.ToString());
                    }

                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return true;
                }
                else
                    return false;
            
        }
    }
}
