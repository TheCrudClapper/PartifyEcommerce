using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.DeliveryTypeDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class DeliveryTypeMappings
    {
        public static DeliveryTypeResponse ToDeliveryTypeResponseDto(this DeliveryType deliveryType)
        {
            return new DeliveryTypeResponse
            {

                Id = deliveryType.Id,
                Price = deliveryType.Price,
                Title = deliveryType.Title
            };
        }
    }
}
