using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.CategoryDto;
using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class CardResponseMappings
    {
        public static CardResponse ToCardResponse(this CategoryResponse dto)
        {
            return new CardResponse
            {
                Id = dto.Id,
                ImageUrl = dto.CategoryImage,
                Title = dto.Name,
            };
        }

        public static CardResponse ToCardResponse(this Offer offer)
        {
            var firstActiveImage = offer.Product.ProductImages.FirstOrDefault(item => item.IsActive);
            return new CardResponse()
            {
                Id = offer.Id,
                ImageUrl = firstActiveImage?.ImagePath,
                Price = offer.Price,
                Title = offer.Product.ProductName,
            };
        }
    }
}
