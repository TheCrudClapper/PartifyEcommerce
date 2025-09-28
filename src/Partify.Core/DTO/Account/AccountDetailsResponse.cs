using CSOS.Core.DTO.Address;

namespace CSOS.Core.DTO.Account
{
    public class AccountDetailsResponse
    {
        public AddressResponse AddressResponse{ get; set; } = null!;
        public AccountResponse AccountResponse { get; set; } = null!;
    }
}
