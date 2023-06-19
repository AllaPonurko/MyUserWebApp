using MyUserWebApp.Models;
using MyUserWebApp.MyRepository;
using MyUserWebApp.ViewModels;

namespace MyUserWebApp.Services
{
    public interface IUserService
    {
        public Task<MyUser> GetUserById(string id);
        public IAsyncEnumerable<MyUser> GetAllUsers();
        public Task<bool> CreateUser(IFormCollection form, RegisterModel model);
        public Task<bool> UpdateUser(IFormFile avatarFile, EditUserViewModel model);
        public Task<bool> DeleteUser(string id);
        public Task<EditUserViewModel> GetProfile(string id);
        public Task<bool>Login(LoginModel model);
        public Task<bool> GetLoginForm(string returnUrl = null);
        public Task<bool> LogOut();
    }
}
