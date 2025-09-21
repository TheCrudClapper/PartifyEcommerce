using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for accessing address data.
    /// </summary>
    public interface IAddressRepository
    {
        /// <summary>
        /// Asynchronously retrieves an <see cref="Address"/> by its ID from the data store.
        /// </summary>
        /// <param name="addressId">The ID of the address to search for.</param>
        /// <returns>The matching <see cref="Address"/> if found; otherwise, <c>null</c>.</returns>
        Task<Address?> GetAddressByIdAsync(int addressId);

        /// <summary>
        /// Asynchronously adds a new address to the system.
        /// </summary>
        /// <param name="address">The address to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added  <see cref="Address"/>
        /// object, including any updates made during the addition process.</returns>
        Task AddAsync(Address address);
    }
}
