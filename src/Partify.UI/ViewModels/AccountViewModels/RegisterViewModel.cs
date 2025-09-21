using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CSOS.UI.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage ="Surname is required")]
        public string Surname { get; set; } = null!;

        [Required(ErrorMessage ="Password is required")]
        [MinLength(6,ErrorMessage ="Password must have at least 6 letters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string? NIP { get; set; }

        public string? Title { get; set; }

        [MinLength(9,ErrorMessage = "Number can't have less than 9 digits")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email in not appropiate format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Palce is required")]
        public string Place { get; set; } = null!;

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "House Number is required")]
        public string HouseNumber { get; set; } = null!;

        [Required(ErrorMessage = "Postal City is required")]
        public string PostalCity { get; set; } = null!;
         
        [Required(ErrorMessage = "Postal Code is required")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; } = null!;

        [Required(ErrorMessage = "Country is required")]
        public string SelectedCountry { get; set; } = null!;

        [BindNever]
        public List<SelectListItem>? CountriesSelectionList { get; set; }
    }
}
