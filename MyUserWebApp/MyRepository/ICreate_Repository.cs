using MyUserWebApp.ViewModels;

namespace MyUserWebApp.MyRepository
{
    public interface ICreate_Repository
    {
        public Task<bool> Register(IFormCollection form, RegisterModel model);
    }
}
