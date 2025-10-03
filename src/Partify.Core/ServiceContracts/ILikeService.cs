using CSOS.Core.DTO.LikedOfferDto;
using CSOS.Core.ResultTypes;

namespace CSOS.Core.ServiceContracts;

public interface ILikeService
{
    /// <summary>
    /// Toggles the like status for the specified offer asynchronously. If the offer is currently liked by the user,
    /// this method will remove the like; otherwise, it will add a like.
    /// </summary>
    /// <param name="offerId">The unique identifier of the offer whose like status is to be toggled. Must be a positive integer.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{LikeResult}"/>
    /// indicating the outcome of the toggle operation, including the new like status.</returns>
    Task<Result<LikeResult>> ToggleLike(int offerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a filtered collection of offers liked by the current user, optionally matching the specified title.
    /// </summary>
    /// <param name="title">The title to filter liked offers by. If null or empty, all liked offers are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a collection of liked offer responses
    /// matching the filter criteria. The collection will be empty if no offers match.</returns>
    Task<Result<IEnumerable<LikedOfferResponse>>> GetFilteredUserLikedOffers(string? title, CancellationToken cancellationToken = default);
}
