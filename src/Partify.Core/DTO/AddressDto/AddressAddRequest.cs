namespace CSOS.Core.DTO.AddressDto
{
    public  class AddressAddRequest
    {
        public string Place { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public int CountryId { get; set; }
        public int? UserId { get; set; }
    }
}
