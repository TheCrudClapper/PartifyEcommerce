using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.AddressDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class AddressMappings
    {
        public static AddressResponse ToAddressResponse(this Address address)
        {
            return new AddressResponse
            {
                HouseNumber = address.HouseNumber,
                Id = address.Id,
                Place = address.Place,
                PostalCity = address.PostalCity,
                PostalCode = address.PostalCode,
                SelectedCountry = address.Country.Id,
                Street = address.Street
            };
        }
    }
}
