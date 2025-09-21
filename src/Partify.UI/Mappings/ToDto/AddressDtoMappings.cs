using CSOS.Core.DTO.AddressDto;
using CSOS.UI.ViewModels.AddressViewModels;

namespace CSOS.UI.Mappings.ToDto
{
    public static class AddressDtoMappings
    {
        public static AddressUpdateRequest ToAddressUpdateRequest(this EditAddressViewModel viewModel)
        {
            return new AddressUpdateRequest()
            {
                Id = viewModel.Id,
                CountryId = int.Parse(viewModel.SelectedCountry),
                HouseNumber = viewModel.HouseNumber,
                Place = viewModel.Place,
                PostalCity = viewModel.PostalCity,
                PostalCode = viewModel.PostalCode,
                Street = viewModel.Street,
            };
        }

        public static AddressAddRequest ToAddressAddRequest(this AddAddressViewModel viewModel)
        {
            return new AddressAddRequest()
            {
                CountryId = int.Parse(viewModel.SelectedCountry),
                HouseNumber = viewModel.HouseNumber,
                Place = viewModel.Place,
                PostalCity = viewModel.PostalCity,
                PostalCode = viewModel.PostalCode,
                Street = viewModel.Street,
            };
        }
    }
}
