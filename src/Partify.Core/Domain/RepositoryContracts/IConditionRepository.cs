using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for accessing product condition data.
    /// </summary>
    public interface IConditionRepository
    {
        /// <summary>
        /// Asynchronously retrieves all product conditions from the database.
        /// </summary>
        /// <returns>A collection of <see cref="Condition"/> entities.</returns>
        Task<IEnumerable<Condition>> GetAllConditionsAsync();
    }
}
