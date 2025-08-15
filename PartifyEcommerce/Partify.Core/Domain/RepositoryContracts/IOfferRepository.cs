using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Helpers;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for managing offer-related operations in the system.
    /// </summary>
    public interface IOfferRepository
    {
        /// <summary>
        /// Asynchronously adds a new offer to the database.
        /// </summary>
        /// <param name="entity">The offer entity to add.</param>
        Task AddAsync(Offer entity);

        /// <summary>
        /// Retrieves an offer with related details for editing by its owner.
        /// </summary>
        /// <param name="id">The ID of the offer.</param>
        /// <param name="userId">The ID of the user (offer owner).</param>
        /// <returns>The offer with its details if found and owned by the user; otherwise, null.</returns>
        Task<Offer?> GetOfferWithDetailsToEditAsync(int id, Guid userId);

        /// <summary>
        /// Retrieves a public, active offer by its ID.
        /// </summary>
        /// <param name="id">The ID of the offer.</param>
        /// <returns>The offer if found and active; otherwise, null.</returns>
        Task<Offer?> GetOfferByIdAsync(int id);

        /// <summary>
        /// Retrieves a limited number of active and non-private offers.
        /// </summary>
        /// <param name="take">The maximum number of offers to retrieve (default is 12).</param>
        /// <returns>A collection of offers.</returns>
        Task<IEnumerable<Offer>> GetOffersByTakeAsync(int take = 12);

        /// <summary>
        /// Retrieves filtered active offers created by a specific user.
        /// </summary>
        /// <param name="title">Optional search phrase for product name.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of matching offers created by the user.</returns>
        Task<IEnumerable<Offer>> GetFilteredUserOffersAsync(string? title, Guid userId);

        /// <summary>
        /// Retrieves filtered public offers based on the specified filter criteria.
        /// </summary>
        /// <param name="filter">The filter parameters including category, price range, search phrase, etc.</param>
        /// <returns>A collection of matching public offers.</returns>
        Task<IEnumerable<Offer>> GetFilteredOffersAsync(OfferFilter filter);

        /// <summary>
        /// Retrieves a user's specific active offer by ID.
        /// </summary>
        /// <param name="id">The ID of the offer.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user's offer if found and active; otherwise, null.</returns>
        Task<Offer?> GetUserOffersByIdAsync(int id, Guid userId);

        /// <summary>
        /// Retrieves a public offer with all related data such as product, images, seller, delivery options.
        /// </summary>
        /// <param name="id">The ID of the offer.</param>
        /// <returns>The offer with full details; otherwise, null.</returns>
        Task<Offer?> GetOfferWithAllDetailsAsync(int id);

        /// <summary>
        /// Retrieves a user's offer with all related data, including product, category, condition, and delivery types.
        /// </summary>
        /// <param name="id">The ID of the offer.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The offer with full details if found and owned by the user; otherwise, null.</returns>
        Task<Offer?> GetOfferWithAllDetailsByUserAsync(int id, Guid userId);

        /// <summary>
        /// Checks whether an active offer with the specified ID exists in the database.
        /// </summary>
        /// <param name="id">The ID of the offer.</param>
        /// <returns>True if the offer exists and is active; otherwise, false.</returns>
        Task<bool> IsOfferInDbAsync(int id);

        /// <summary>
        /// Gets the total count of active and public (non-private) offers.
        /// </summary>
        /// <returns>The number of non-private offers in the database.</returns>
        Task<int> GetNonPrivateOfferCount();
    }
}
