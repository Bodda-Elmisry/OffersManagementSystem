using System.ComponentModel.DataAnnotations;

namespace OffersManagementSystem.Web.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Passord is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
    }
}
