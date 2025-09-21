using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CSOS.Core.DTO.AccountDto
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "First Name is required"), DisplayName("First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Remote(controller: "Account", action: "IsEmailAlreadyTaken", ErrorMessage = "Email is already taken !")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        [Required, Compare("Password", ErrorMessage = "Passwords doesn't match"), DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
      
    }
}
