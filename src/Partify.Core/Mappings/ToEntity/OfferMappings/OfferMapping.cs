using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.OfferDto;

namespace CSOS.Core.Mappings.ToDomainEntity.OfferMappings
{
    public static class OfferMapping
    {
        public static Offer ToOfferEntity(this OfferAddRequest dto, Guid userId)
        {
            return new Offer()
            {
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                Price = dto.Price,
                SellerId = userId,
                StockQuantity = dto.StockQuantity,
                IsOfferPrivate = dto.IsOfferPrivate,
            };
        }

    }
}
