using CSOS.Core.DTO;
using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.ServiceContracts
{
    public interface IConditionGetterService
    {
        /// <summary>
        /// Retrieves a collection of product conditions formatted as select list items.
        /// </summary>
        /// <remarks>This method is typically used to populate UI elements such as dropdowns with  product
        /// condition options. The returned collection may be empty if no product  conditions are available.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  enumerable collection of
        /// <see cref="SelectListItemDto"/> objects, where each item  represents a product condition suitable for use in
        /// a dropdown or selection control.</returns>
        Task<IEnumerable<SelectListItemDto>> GetProductConditionsAsSelectList();
    }
}
