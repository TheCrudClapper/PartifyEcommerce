using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts;

public interface ILikeOfferRepository
{
    /// <summary>
    /// Adds a new like for the specified offer or reactivates an existing like if it was previously deactivated.
    /// </summary>
    /// <remarks>If the specified offer has already been liked but the like is inactive, this method
    /// reactivates the like. Otherwise, it creates a new like for the offer.</remarks>
    /// <param name="likedOffer">The offer to be liked, represented by a <see cref="LikedOffer"/> object. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddOrReactivateLikeAsync(LikedOffer likedOffer, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a liked for offer for a specific user.
    /// </summary>
    /// <remarks>Use this method to retrieve details of a specific liked offer for a user. Ensure that
    /// the provided identifiers are valid and correspond to existing records.</remarks>
    /// <param name="likedOffer">The unique identifier of the liked offer to retrieve.</param>
    /// <param name="userId">The unique identifier of the user associated with the liked offer.</param>
    /// <returns>A <see cref="LikedOffer"/> object representing the liked offer if found; otherwise, <see langword="null"/>.</returns>
    Task<LikedOffer?> GetLikedOfferAsync(int likedOffer, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrives all liked offers for specific user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<Offer>> GetAllUserLikedOffersAsync(Guid userId, string? title, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates like for specific user and offer
    /// </summary>
    /// <param name="likedOffer"></param>
    /// <returns></returns>
    Task RemoveLikeAsync(LikedOffer likedOffer, CancellationToken cancellationToken = default);
}
