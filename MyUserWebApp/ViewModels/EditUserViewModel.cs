using MyUserWebApp.MyRepository;

namespace MyUserWebApp.ViewModels
{
    public class EditUserViewModel: IItem
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public string? AboutMe { get; set; }

    }
}
