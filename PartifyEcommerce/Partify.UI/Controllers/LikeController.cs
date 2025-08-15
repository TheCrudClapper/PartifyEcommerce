using CSOS.Core.DTO.LikedOfferDto;
using CSOS.Core.ServiceContracts;
using CSOS.Core.Services;
using CSOS.UI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    public class LikeController : Controller
    {
        private readonly ILikeService _likeService;
        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        public async Task<JsonResult> LikeOffer([FromRoute] int id)
        {
            var result = await _likeService.ToggleLike(id);

            if (result.IsFailure)
                return Json(new JsonResponseModel { Message = result.Error.Description, Success = false });

            return Json(new JsonResponseModel<LikeResponse>
            {
                Success = true,
                Message = result.Value.Message,
                Data = result.Value
            });
        }
    }
}
