using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.ViewModels;
using NPOI.SS.Formula.Functions;

namespace MyUserWebApp.MyRepository
{
    public interface IRepositoryUpdate
    {
        public Task<EditUserViewModel> GetViewProfile(string id);
        public Task<IItem> Edit(IFormFile avatarFile, EditUserViewModel model);
    }
}
