namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a unit of work that coordinates saving changes across multiple repositories.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Asynchronously saves all changes made in the current unit of work to the database.
        /// </summary>
        /// <param name="cancellation">Optional cancellation token to cancel the operation.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellation = default);
    }
}
