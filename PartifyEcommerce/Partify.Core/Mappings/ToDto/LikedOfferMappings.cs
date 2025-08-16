using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.LikedOfferDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class LikedOfferMappings
    {
        public static LikedOfferResponse ToLikeOfferResponse(this Offer offer)
        {
            return new LikedOfferResponse(offer.Id,
                offer.Product.ProductName,
                offer.Price,
                offer.DateCreated,
                offer.StockQuantity,
                offer.Product.ProductImages
                .Select(item => item.ImagePath)
                .FirstOrDefault());
        }
    }
}
