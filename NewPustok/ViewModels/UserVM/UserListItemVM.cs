using System.ComponentModel.DataAnnotations;

namespace NewPustok.ViewModels.UserVM
{
    public class UserListItemVM
    {
        public string ImageUrl { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
