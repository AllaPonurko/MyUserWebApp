using MyUserWebApp.Models;
using MyUserWebApp.MyRepository;
using MyUserWebApp.ViewModels;
using System.Collections.Generic;

namespace MyUserWebApp.Services
{
    public class UserService : IUserService
    {
        private CRUD_Repository _cRUD;
        public UserService(CRUD_Repository cRUD)
        {
            _cRUD = cRUD;
        }

        public async Task<bool> CreateUser(IFormCollection form, RegisterModel model)
        {
           return await _cRUD.Register(form, model) ;     
        }

        public async Task<bool> DeleteUser(string userId)
        {
            return await _cRUD.Delete(userId);
        }

        public  IAsyncEnumerable<MyUser> GetAllUsers()
        {
           return (IAsyncEnumerable<MyUser>)_cRUD.GetAllUsers();
        }

        public Task<bool> GetLoginForm(string returnUrl = null)
        {
            throw new NotImplementedException();
        }

        public async Task<EditUserViewModel> GetProfile(string id)
        {
            return await _cRUD.GetViewProfile(id);
        }

        public async Task<MyUser> GetUserById(string id)
        {
            return await _cRUD.FindMyUserById(id);
        }

        public async Task<bool> Login(LoginModel model)
        {
           return await _cRUD.LoginAsync(model);
        }

        public async Task<bool> LogOut()
        {
            return await _cRUD.LogOut();
        }

        public async Task<bool> UpdateUser(IFormFile avatarFile, EditUserViewModel model)
        {
            if (await _cRUD.Edit(avatarFile, model)!= model)
                return true;
            return false;
        }
    }
}
