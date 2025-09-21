using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for accessing country data.
    /// </summary>
    public interface ICountryRepository
    {
        /// <summary>
        /// Asynchronously retrieves all countries from the database.
        /// </summary>
        /// <returns>A collection of <see cref="Country"/> entities.</returns>
        Task<IEnumerable<Country>> GetAllCountriesAsync();
    }
}
