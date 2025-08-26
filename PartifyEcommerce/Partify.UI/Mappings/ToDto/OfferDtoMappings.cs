using CSOS.Core.DTO.OfferDto;
using CSOS.UI.ViewModels.OfferViewModels;

namespace CSOS.UI.Mappings.ToDto
{
    public static class OfferDtoMappings
    {
        public static OfferUpdateRequest ToEditOfferDto(this EditOfferViewModel viewModel)
        {
            return new OfferUpdateRequest()
            {
                Id = viewModel.Id,
                Description = viewModel.Description,
                IsOfferPrivate = viewModel.IsOfferPrivate,
                Price = viewModel.Price,
                ProductName = viewModel.ProductName,
                StockQuantity = viewModel.StockQuantity,
                UploadedImages = viewModel.UploadedImages,
                ImagesToDeleteIds = viewModel.ImagesToDeleteIds?
                    .Where(item => int.TryParse(item, out _))
                    .Select(int.Parse).ToList(),
                SelectedParcelLocker = viewModel.SelectedParcelLocker,
                SelectedProductCondition = int.Parse(viewModel.SelectedProductCondition),
                SelectedProductCategory = int.Parse(viewModel.SelectedProductCategory),
                SelectedOtherDeliveries = viewModel.SelectedOtherDeliveries,
            };
        }
    }
}
