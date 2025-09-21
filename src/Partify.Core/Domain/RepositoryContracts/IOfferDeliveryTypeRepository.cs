using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for managing offer delivery type associations.
    /// </summary>
    public interface IOfferDeliveryTypeRepository
    {
        /// <summary>
        /// Asynchronously adds a single <see cref="OfferDeliveryType"/> entity to the database.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(OfferDeliveryType entity);

        /// <summary>
        /// Asynchronously adds a collection of <see cref="OfferDeliveryType"/> entities to the database.
        /// </summary>
        /// <param name="entities">The collection of entities to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddRangeAsync(IEnumerable<OfferDeliveryType> entities);
    }
}
