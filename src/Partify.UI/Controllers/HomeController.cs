using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.HomePageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IOfferService offerService,
            ICategoryGetterService categoryGetterService,
            ILogger<HomeController> logger,
            IPictureHandlerService pictureHandlerService)
        {
            _offerService = offerService;
            _categoryGetterService = categoryGetterService;
            _pictureHandlerService = pictureHandlerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("HomeController - GET Index Method called");
            var viewModel = new IndexPageViewModel()
            {
                Cards = (await _offerService.GetIndexPageOffers()).Select((item => item.ToCardViewModel(_pictureHandlerService))),
                Categories = (await _categoryGetterService.GetProductCategoriesAsSelectList()).ToSelectListItem(),
                CategoriesSlider = (await _categoryGetterService.GetProductCategoriesAsCardResponse()).Select(item => item.ToCardViewModel(_pictureHandlerService)),
                BestDeals = (await _offerService.GetDealsOfTheDay()).Select(item=>item.ToCardViewModel(_pictureHandlerService)),
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogInformation("HomeController - GET Error Method called");
            IExceptionHandlerFeature? handler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (handler != null && handler.Error != null)
                ViewBag.Error = handler.Error.Message;

            return View();
        }
    }
}


