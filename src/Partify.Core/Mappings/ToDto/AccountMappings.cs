using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.DTO.AccountDto;
using CSOS.Core.DTO.AddressDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class AccountMappings
    {
        public static AccountResponse ToAccountResponse(this ApplicationUser account)
        {
            return new AccountResponse()
            {
                Id = account.Id,
                FirstName = account.FirstName,
                NIP = account.NIP,
                PhoneNumber = account.PhoneNumber,
                Surname = account.Surname,
                Title = account.Title,
            };
        }
        public static UserAddressDetailsResponse ToUserAddressDetailsResponse(this ApplicationUser account)
        {
            return new UserAddressDetailsResponse()
            {
                CustomerName = account.FirstName + " " + account.Surname,
                PostalInfo = account.Address.PostalCode + " " + account.Address.PostalCity,
                Address = account.Address.Place + ",  " + account.Address.Street + " " + account.Address.HouseNumber,
                PhoneNumber = account?.PhoneNumber,
            };
        }

        public static AddressResponse ToAddressResponse(this ApplicationUser account)
        {
            return new AddressResponse()
            {
                Id = account.Address.Id,
                HouseNumber = account.Address.HouseNumber,
                Place = account.Address.Place,
                PostalCity = account.Address.PostalCity,
                PostalCode = account.Address.PostalCode,
                Street = account.Address.Street,
                SelectedCountry = account.Address.CountryId,
            };
        }
    }
}
