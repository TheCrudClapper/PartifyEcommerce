using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.ServiceContracts
{
    public interface ISortingOptionService
    {
        /// <summary>
        /// Retrieves available sorting options for offer browsing.
        /// </summary>
        /// <returns>List of sorting options.</returns>
        IEnumerable<SelectListItemDto> GetSortingOptions();
    }
}
