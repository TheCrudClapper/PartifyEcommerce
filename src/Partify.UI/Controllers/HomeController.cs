using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.HomePageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IOfferService _offerService;
    private readonly IPictureHandlerService _pictureHandlerService;
    private readonly ICategoryGetterService _categoryGetterService;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IOfferService offerService,
        ICategoryGetterService categoryGetterService,
        ILogger<HomeController> logger,
        IPictureHandlerService pictureHandlerService,
        IWebHostEnvironment env)
    {
        _offerService = offerService;
        _categoryGetterService = categoryGetterService;
        _pictureHandlerService = pictureHandlerService;
        _logger = logger;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        _logger.LogInformation("HomeController - GET Index Method called");
        var viewModel = new IndexPageViewModel()
        {
            Cards = (await _offerService.GetIndexPageOffers(cancellationToken))
                .Select((item => item.ToCardViewModel(_pictureHandlerService))),

            Categories = (await _categoryGetterService.GetProductCategoriesAsSelectList(cancellationToken))
                .ToSelectListItem(),

            CategoriesSlider = (await _categoryGetterService.GetProductCategoriesAsCardResponse(cancellationToken))
                .Select(item => item.ToCardViewModel(_pictureHandlerService)),

            BestDeals = (await _offerService.GetDealsOfTheDay(cancellationToken))
                .Select(item => item.ToCardViewModel(_pictureHandlerService)),
        };
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Privacy(CancellationToken cancellationToken)
    {
        return View();
    }

    [HttpGet]
    public IActionResult AboutUs(CancellationToken cancellationToken)
    {
        return View();
    }

    [HttpGet]
    [Route("/Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(CancellationToken cancellationToken)
    {
        _logger.LogInformation("HomeController - GET Error Method called");
        IExceptionHandlerFeature? handler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var message = handler?.Error != null && _env.IsDevelopment()
            ? handler.Error.Message
            : "500 - Internal Server Error";

        return View("Error", message);
    }
}


