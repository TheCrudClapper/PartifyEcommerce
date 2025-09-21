using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.UI.ViewModels.DeliveryTypeViewModels;
using CSOS.UI.ViewModels.SharedViewModels;
namespace CSOS.UI.Mappings.ToViewModel
{
    public static class DeliveryViewModelMappings
    {
        public static DeliveryTypeViewModel ToDeliveryTypeViewModel(this DeliveryTypeResponse deliveryType)
        {
            return new DeliveryTypeViewModel
            {
                Id = deliveryType.Id,
                Price = deliveryType.Price,
                Title = deliveryType.Title,
            };
        }
    }
}
