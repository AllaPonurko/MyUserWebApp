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

        public Task CreateUser(MyUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUser(string id)
        {
            return await _cRUD.Delete(id);
        }

        public  IAsyncEnumerable<MyUser> GetAllUsers()
        {
           return (IAsyncEnumerable<MyUser>)_cRUD.GetAllUsers();
        }

        public async Task<MyUser> GetUserById(string id)
        {
            return await _cRUD.FindMyUserById(id);
        }

        public async Task<MyUser> UpdateUser(IFormFile avatarFile, EditUserViewModel model)
        {
           return (MyUser)await _cRUD.Edit(avatarFile,model);
        }
    }
}
