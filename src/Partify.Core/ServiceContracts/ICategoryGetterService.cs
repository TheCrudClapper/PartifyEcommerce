using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.ServiceContracts
{
    public interface ICategoryGetterService
    {
        /// <summary>
        /// Retrieves a collection of product categories formatted as card responses.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains  an enumerable collection of
        /// <see cref="CardResponse"/> objects, each representing  a product category.</returns>
        Task<IEnumerable<CardResponse>> GetProductCategoriesAsCardResponse();
        /// <summary>
        /// Retrieves a collection of product categories formatted as select list items.
        /// </summary>
        /// <remarks>This method is typically used to populate UI elements such as dropdown lists with
        /// product  categories. The returned collection will be empty if no product categories are available.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IEnumerable{T}> of
        /// <SelectListItemDto> objects, where each item  represents a product category suitable for use in a dropdown
        /// or selection control.</returns>
        Task<IEnumerable<SelectListItemDto>> GetProductCategoriesAsSelectList();

    }
}
