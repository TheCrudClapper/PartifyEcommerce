namespace CSOS.UI.ViewModels.AddressViewModels
{
    public class UserAddressDetailsViewModel
    {
        public int AddressId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostalInfo { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
    }
}
