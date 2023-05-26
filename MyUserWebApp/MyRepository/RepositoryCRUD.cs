using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.Models;
using MyUserWebApp.ViewModels;
using System.Security.Claims;

namespace MyUserWebApp.MyRepository
{
    public class RepositoryCRUD : Controller, IRepositoryDelete,IRepositoryUpdate
    {
        UserManager<MyUser> _userManager;

        public RepositoryCRUD(UserManager<MyUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            EditUserViewModel model = new EditUserViewModel();
            if (User.Identity.IsAuthenticated)
            {
                model.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            MyUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }  
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // Успешно удалено
                // Дополнительная логика или перенаправление на другую страницу
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Ошибка при удалении пользователя
                // Обработка ошибки или перенаправление на другую страницу
                return RedirectToAction("Error", "Home");
            }
        }


        //[HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Id = model.Id;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.AboutMe = model.AboutMe;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }
    }
}
