using CSOS.Core.DTO;
using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.ServiceContracts
{
    public interface IDeliveryTypeGetterService
    {
        /// <summary>
        /// Retrieves a list of delivery types that are parcel locker options.
        /// </summary>
        /// <returns>A list of <see cref="DeliveryTypeResponse"/> representing parcel locker delivery types.</returns>
        Task<IEnumerable<DeliveryTypeResponse>> GetParcelLockerDeliveryTypes();

        /// <summary>
        /// Retrieves delivery types that are not parcel locker options, returned as select list items.
        /// </summary>
        /// <returns>A list of <see cref="SelectListItemDto"/> representing other delivery types.</returns>
        Task<IEnumerable<SelectListItemDto>> GetOtherDeliveryTypes();

        /// <summary>
        /// Retrieves all active delivery types available in the database.
        /// </summary>
        /// <returns>A list of <see cref="SelectListItemDto"/> representing all available delivery types.</returns>
        Task<IEnumerable<SelectListItemDto>> GetAllDeliveryTypesAsSelectionList();
    }
}
