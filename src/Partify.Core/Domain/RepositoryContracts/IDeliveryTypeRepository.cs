using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for accessing delivery type data.
    /// </summary>
    public interface IDeliveryTypeRepository
    {
        /// <summary>
        /// Asynchronously retrieves all active delivery types from the database.
        /// </summary>
        /// <returns>A collection of <see cref="DeliveryType"/> entities.</returns>
        Task<IEnumerable<DeliveryType>> GetAllDeliveryTypesAsync();
    }
}
