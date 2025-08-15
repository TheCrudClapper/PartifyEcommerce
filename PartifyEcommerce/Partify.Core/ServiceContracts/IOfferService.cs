using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.LikedOfferDto;
using CSOS.Core.DTO.OfferDto;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Helpers;
using CSOS.Core.ResultTypes;

namespace CSOS.Core.ServiceContracts
{
    public interface IOfferService
    {
        /// <summary>
        /// Adds a new offer for the current user to the database.
        /// </summary>
        /// <param name="addRequest">DTO containing offer data.</param>
        /// <returns>Result indicating success or failure.</returns>
        Task<Result> Add(OfferAddRequest addRequest);

        /// <summary>
        /// Edits an existing offer and saves changes to the database.
        /// </summary>
        /// <param name="updateRequest">DTO containing updated offer data.</param>
        /// <returns>Result indicating success or failure.</returns>
        Task<Result> Edit(OfferUpdateRequest updateRequest);

        /// <summary>
        /// Retrieves a specific offer by its ID.
        /// </summary>
        /// <param name="id">The ID of the offer.</param>
        /// <returns>Result containing the offer data if found.</returns>
        Task<Result<OfferResponse>> GetOffer(int id);

        /// <summary>
        /// Retrieves all user offers with optional filtering by product title.
        /// </summary>
        /// <param name="title">Optional title to filter offers by.</param>
        /// <returns>List of offers matching the filter.</returns>
        Task<IEnumerable<UserOfferResponse>> GetFilteredUserOffers(string? title);

        /// <summary>
        /// Deletes an offer by its ID.
        /// </summary>
        /// <param name="id">The ID of the offer to delete.</param>
        /// <returns>Result indicating success or failure.</returns>
        Task<Result> DeleteOffer(int id);

        /// <summary>
        /// Retrieves offer details for editing, typically used to populate a form.
        /// </summary>
        /// <param name="id">The ID of the offer.</param>
        /// <returns>Result containing the editable offer data.</returns>
        Task<Result<EditOfferResponse>> GetOfferForEdit(int id);

        /// <summary>
        /// Checks whether an offer exists and belongs to the current user.
        /// </summary>
        /// <param name="id">The ID of the offer.</param>
        /// <returns>True if the offer exists and is accessible; otherwise, false.</returns>
        Task<bool> DoesOfferExist(int id);

        /// <summary>
        /// Retrieves offers based on advanced filter options.
        /// </summary>
        /// <param name="filter">Filter parameters for offer searching.</param>
        /// <returns>Filtered offers suitable for offer browsing views.</returns>
        Task<IEnumerable<OfferIndexResponse>> GetFilteredOffers(OfferFilter filter);

        /// <summary>
        /// Retrieves highlighted offers for display on the main index page.
        /// </summary>
        /// <returns>List of offers formatted for main page display.</returns>
        Task<IEnumerable<CardResponse>> GetIndexPageOffers();

        /// <summary>
        /// Retrieves current deals of the day.
        /// </summary>
        /// <returns>List of offers labeled as deals of the day.</returns>
        Task<IEnumerable<CardResponse>> GetDealsOfTheDay();
    }
}
