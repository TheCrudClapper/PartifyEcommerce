using CSOS.Core.DTO.AccountDto;
using CSOS.UI.ViewModels.AccountViewModels;

namespace CSOS.UI.Mappings.ToDto
{
    public static class AccountDtoMappings
    {
        public static AccountUpdateRequest ToAccountUpdateRequest(this UserDetailsViewModel viewModel)
        {
            return new AccountUpdateRequest()
            {
                FirstName = viewModel.FirstName,
                Surname = viewModel.Surname,
                NIP = viewModel.NIP,
                PhoneNumber = viewModel.PhoneNumber,
                Title = viewModel.Title,
            };
        }
    }
}
