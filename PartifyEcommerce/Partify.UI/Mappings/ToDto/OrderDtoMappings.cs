using CSOS.Core.DTO.OfferDto;
using CSOS.UI.ViewModels.OfferViewModels;

namespace CSOS.UI.Mappings.ToDto
{
    public static  class OrderDtoMappings
    {
        public static OfferAddRequest ToAddOfferDto(this AddOfferViewModel viewModel)
        {
            return new OfferAddRequest()
            {
                Description = viewModel.Description,
                IsOfferPrivate = viewModel.IsOfferPrivate,
                Price = viewModel.Price,
                ProductName = viewModel.ProductName,
                SelectedOtherDeliveries = viewModel.SelectedOtherDeliveries,
                SelectedParcelLocker = viewModel.SelectedParcelLocker,
                UploadedImages = viewModel.UploadedImages,
                SelectedProductCategory = int.Parse(viewModel.SelectedProductCategory),
                SelectedProductCondition = int.Parse(viewModel.SelectedProductCondition),
                StockQuantity = viewModel.StockQuantity,
            };
        }
    }
}
