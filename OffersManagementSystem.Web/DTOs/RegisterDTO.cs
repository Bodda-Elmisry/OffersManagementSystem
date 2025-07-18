using System.ComponentModel.DataAnnotations;

namespace OffersManagementSystem.Web.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
    }
}
