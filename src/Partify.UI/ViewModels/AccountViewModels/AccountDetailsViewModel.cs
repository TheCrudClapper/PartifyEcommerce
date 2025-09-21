using CSOS.Core.DTO.AccountDto;
using CSOS.UI.ViewModels.AddressViewModels;

namespace CSOS.UI.ViewModels.AccountViewModels
{
    public class AccountDetailsViewModel
    {
        public UserDetailsViewModel UserDetails { get; set; } = null!;
        public EditAddressViewModel? EditAddress { get; set; }
        public AddAddressViewModel? AddAddressViewModel { get; set; }
        public PasswordChangeRequest PasswordChangeRequest { get; set; } = null!;
    }
}
