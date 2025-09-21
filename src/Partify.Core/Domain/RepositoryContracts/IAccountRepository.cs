using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for managing application user accounts.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Asynchronously adds a new user to the data storage.
        /// </summary>
        /// <param name="entity">The user entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(ApplicationUser entity);

        /// <summary>
        /// Asynchronously checks if a user with the given email exists in the database.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns><c>true</c> if a user with the specified email exists; otherwise, <c>false</c>.</returns>
        Task<bool> IsUserByEmailInDatabaseAsync(string email);

        /// <summary>
        /// Asynchronously retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The matching <see cref="ApplicationUser"/> if found; otherwise, <c>null</c>.</returns>
        Task<ApplicationUser?> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Asynchronously retrieves a user with their address eagerly loaded, by user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The matching <see cref="ApplicationUser"/> with address if found; otherwise, <c>null</c>.</returns>
        Task<ApplicationUser?> GetUserWithAddressAsync(Guid userId);

        /// <summary>
        /// Determines whether the specified email address is already in use.
        /// </summary>
        /// <param name="email">The email address to check. Must not be null or empty.</param>
        /// <returns><see langword="true"/> if the email address is already taken; otherwise, <see langword="false"/>.</returns>
        Task<bool> IsEmailTakenAsync(string email);
    }
}
