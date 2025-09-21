using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for managing product-related operations.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Asynchronously adds a new product to the database.
        /// </summary>
        /// <param name="entity">The product entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Product entity);
    }
}
