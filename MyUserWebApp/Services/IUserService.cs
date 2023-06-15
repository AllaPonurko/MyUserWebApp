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
        public Task<MyUser> UpdateUser(IFormFile avatarFile, EditUserViewModel model);
        public Task<bool> DeleteUser(string id);
    }
}
