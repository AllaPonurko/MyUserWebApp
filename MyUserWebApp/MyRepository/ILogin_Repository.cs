using MyUserWebApp.ViewModels;

namespace MyUserWebApp.MyRepository
{
    public interface ILogin_Repository
    {
       //public Task<bool>GetLoginForm(string returnUrl = null);
        public Task<bool> LoginAsync(LoginModel model);
    }
}
