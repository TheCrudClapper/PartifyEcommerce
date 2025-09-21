namespace CSOS.Core.DTO.AddressDto
{
    public class AddressResponse
    {
        public int Id { get; set; }
        public string Place { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public int SelectedCountry { get; set; }
        
        
        public AddressUpdateRequest ToAddressUpdateRequest(AddressResponse response)
        {
            return new AddressUpdateRequest()
            {
                Id = response.Id,
                Place = response.Place,
                Street = response.Street,
                HouseNumber = response.HouseNumber,
                PostalCity = response.PostalCity,
                CountryId = SelectedCountry,
                PostalCode = response.PostalCode
            };
        }
        
    }
}

