using System.ComponentModel.DataAnnotations;
namespace CSOS.UI.ViewModels.AccountViewModels
{
    public class UserDetailsViewModel
    {
        [Required(ErrorMessage = "Enter your first name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Enter your Surname name")]
        public string Surname { get; set; } = null!;

        [Phone( ErrorMessage = "Enter valid phone number")]
        public string? PhoneNumber { get; set; }

        [StringLength(12, ErrorMessage = "NIP should not be longer than 12 characters !")]
        public string? NIP { get; set; }

        [StringLength(40, ErrorMessage = "Title should not be longer than 20 characters !")]
        public string? Title { get; set; }
    }
}
