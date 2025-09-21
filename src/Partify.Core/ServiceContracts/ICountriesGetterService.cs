using CSOS.Core.DTO;
using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.ServiceContracts
{
    public interface ICountriesGetterService
    {   
        /// <summary>
        /// Retrieves a list of countries formatted as selection items for use in dropdowns or other UI components.
        /// </summary>
        /// <remarks>The returned collection is typically used to populate UI elements such as dropdown
        /// lists. The caller is  responsible for awaiting the task and handling any exceptions that may occur during
        /// the operation.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of 
        /// <see cref="SelectListItemDto"/> objects, where each item represents a country with its display text and
        /// value.</returns>
        Task<IEnumerable<SelectListItemDto>> GetCountriesSelectionList();
    }
}
