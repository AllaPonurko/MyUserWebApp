using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.ViewModels;

namespace MyUserWebApp.MyRepository
{
    public interface IRepositoryUpdate
    {
        public Task<IActionResult> Edit(EditUserViewModel model);
    }
}
