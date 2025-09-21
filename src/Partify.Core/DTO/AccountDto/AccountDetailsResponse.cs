using CSOS.Core.DTO.AddressDto;

namespace CSOS.Core.DTO.AccountDto
{
    public class AccountDetailsResponse
    {
        public AddressResponse AddressResponse{ get; set; } = null!;
        public AccountResponse AccountResponse { get; set; } = null!;
    }
}
