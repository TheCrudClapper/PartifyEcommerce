using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.CartDto;
using CSOS.Core.DTO.CartItemDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class CartMappings
    {
        private const string DefaultImagePath = "wwwroot/images/no-image.png";
        public static CartResponseDto ToCartResponseDto(this Cart cart)
        {
            return new CartResponseDto()
            {
                CartItems = cart.CartItems.Where(item => item.IsActive)
                    .Select(item => new CartItemResponse
                    {
                        Category = item.Offer.Product.ProductCategory.Name,
                        Condition = item.Offer.Product.Condition.ConditionTitle,
                        DateAdded = item.DateCreated,
                        Id = item.Id,
                        Price = item.Offer.Price,
                        Quantity = item.Quantity,
                        Title = item.Offer.Product.ProductName,
                        ImageUrl = item.Offer.Product.ProductImages
                            .FirstOrDefault(item => item.IsActive)?.ImagePath ?? DefaultImagePath,
                        OfferId = item.OfferId,

                    }).ToList(),
                TotalCartValue = cart.TotalCartValue ?? 0,
                TotalDeliveryValue = cart.MinimalDeliveryValue ?? 0,
                TotalItemsValue = cart.TotalItemsValue ?? 0,
            };
        }
    }
}
