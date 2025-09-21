using CSOS.Core.DTO.AccountDto;
using CSOS.UI.ViewModels.AccountViewModels;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class AccountMappings
    {
        public static UserDetailsViewModel ToUserDetailsViewModel(this AccountResponse dto)
        {
            return new UserDetailsViewModel()
            {
                FirstName = dto.FirstName,
                NIP = dto.NIP,
                PhoneNumber = dto.PhoneNumber,
                Surname = dto.Surname,
                Title = dto.Title,
            };
        }
    }
}
