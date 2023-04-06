using Microsoft.AspNetCore.Identity;
using MyUserWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace MyUserWebApp.ViewModels
{
    public class LoginModel
    {
        
        public string ReturnUrl{ get; set; }

        [Required]
        [Display(Name = "Email")]

        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        

    }
}
