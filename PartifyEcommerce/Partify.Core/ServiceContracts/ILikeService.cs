using CSOS.Core.DTO.LikedOfferDto;
using CSOS.Core.ResultTypes;

namespace CSOS.Core.ServiceContracts
{
    public interface ILikeService
    {
        Task<Result<LikeResponse>> ToggleLike(int offerId);
    }
}
