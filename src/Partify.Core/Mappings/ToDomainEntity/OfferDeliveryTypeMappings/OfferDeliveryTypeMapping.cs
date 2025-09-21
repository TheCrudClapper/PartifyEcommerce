using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.DtoContracts;

namespace CSOS.Core.Mappings.ToDomainEntity.OfferDeliveryTypeMappings
{
    public static class OfferDeliveryTypeMapping
    {
        public static OfferDeliveryType ToOfferDeliveryTypeEntity(this IOfferDeliveryDto dto, Offer offer)
        {
            return new OfferDeliveryType
            {
                DateCreated = DateTime.UtcNow,
                IsActive = true,
                Offer = offer,
                DeliveryTypeId = dto.SelectedParcelLocker!.Value,
            };
        }

        public static OfferDeliveryType ToOfferDeliveryTypeEntity(this int deliveryId, Offer offer)
        {
            return new OfferDeliveryType()
            {
                DeliveryTypeId = deliveryId,
                Offer = offer,
                IsActive = true,
                DateCreated = DateTime.Now,
            };
        }
    }
}
