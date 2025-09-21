using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.AccountDto;
using CSOS.Core.DTO.AddressDto;

namespace CSOS.Core.Mappings.ToDomainEntity.AddressMappings
{
    public static class AddressMappings
    {
        public static Address ToAddressEntity(this AddressAddRequest request, Guid? userId)
        {
            return new Address()
            {
                Place = request.Place,
                Street = request.Street,
                PostalCity = request.PostalCity,
                PostalCode = request.PostalCode,
                HouseNumber = request.HouseNumber,
                CountryId = request.CountryId,
                UserId = userId,
                IsActive = true,
                DateCreated = DateTime.UtcNow,
            };
        }
    }
}
