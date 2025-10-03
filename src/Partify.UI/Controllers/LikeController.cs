using CSOS.Core.DTO.LikedOfferDto;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers;

public class LikeController : Controller
{
    private readonly ILikeService _likeService;
    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost]
    public async Task<JsonResult> LikeOffer([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _likeService.ToggleLike(id, cancellationToken);

        if (result.IsFailure)
            return Json(new JsonResponseModel { Message = result.Error.Description, Success = false });

        return Json(new JsonResponseModel<LikeResult>
        {
            Success = true,
            Message = result.Value.Message,
            Data = result.Value
        });
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? title, CancellationToken cancellationToken)
    {
        var result = await _likeService.GetFilteredUserLikedOffers(title, cancellationToken);
        if(result.IsFailure)
            return View("Error", result.Error.Description);

        //return View(result.Value);
        return View("Error", "This module is still under development");
    }

    [HttpGet]
    public async Task<IActionResult> FilterLikedOffers(string? title, CancellationToken cancellationToken)
    {
        var result = await _likeService.GetFilteredUserLikedOffers(title, cancellationToken);
        if (result.IsFailure)
            return View("Error", result.Error.Description);

        return PartialView(result.Value);
    }
}
