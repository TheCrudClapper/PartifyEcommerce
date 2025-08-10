using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.CartDto;
using CSOS.UI.ViewModels.CartItemViewModels;
using CSOS.UI.ViewModels.CartViewModels;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class CartViewModelMappings
    {
        public static CartViewModel ToCartViewModel(this CartResponseDto dto, IConfigurationReader configuration)
        {
            return new CartViewModel()
            {
                TotalItemsValue = dto.TotalItemsValue,
                TotalCartValue = dto.TotalCartValue,
                TotalDeliveryValue = dto.TotalDeliveryValue,
                CartItems = dto.CartItems.Select(item => new CartItemViewModel()
                {
                    Category = item.Category,
                    Condition = item.Condition,
                    DateAdded = item.DateAdded,
                    Id = item.Id,
                    ImageUrl = string.IsNullOrWhiteSpace(item.ImageUrl)
                    ? configuration.DefaultPicturePlaceholder
                    : item.ImageUrl,
                    OfferId = item.OfferId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Title = item.Title
                }).ToList(),
            };
        }
    }
}
