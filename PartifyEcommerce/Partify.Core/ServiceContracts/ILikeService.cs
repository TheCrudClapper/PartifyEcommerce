using CSOS.Core.DTO.LikedOfferDto;
using CSOS.Core.ResultTypes;

namespace CSOS.Core.ServiceContracts
{
    public interface ILikeService
    {
        Task<Result<LikeResult>> ToggleLike(int offerId);

        Task<Result<IEnumerable<LikedOfferResponse>>> GetFilteredUserLikedOffers(string? title);
    }
}
