using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for accessing product category data.
    /// </summary>
    public interface IProductCategoryRepository
    {
        /// <summary>
        /// Asynchronously retrieves all active product categories from the database.
        /// </summary>
        /// <returns>A collection of <see cref="ProductCategory"/> entities.</returns>
        Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync();
    }
}
