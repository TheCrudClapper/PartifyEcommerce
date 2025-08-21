using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.OfferDto;
using CSOS.Core.Helpers;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Filters;
using CSOS.UI.Helpers;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.OfferViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IDeliveryTypeGetterService _deliveryTypeGetterService;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly OfferViewModelInitializer _offerViewModelInitializer;
        private readonly IProductImageService _productImageService;
        private readonly IConfigurationReader _configurationReader;
        private readonly ISortingOptionService _sortingOptionService;
        private readonly ILogger<OfferController> _logger;
        private readonly IAccountService _accountService;
        public OfferController(IOfferService offerService,
            IDeliveryTypeGetterService deliveryTypeGetterService,
            IPictureHandlerService pictureHandlerService,
            OfferViewModelInitializer offerViewModelInitializer,
            IProductImageService productImageService,
            ISortingOptionService sortingOptionService,
            IConfigurationReader configurationReader,
            ILogger<OfferController> logger,
            IAccountService accountService)
        {
            _offerService = offerService;
            _pictureHandlerService = pictureHandlerService;
            _offerViewModelInitializer = offerViewModelInitializer;
            _productImageService = productImageService;
            _sortingOptionService = sortingOptionService;
            _configurationReader = configurationReader;
            _deliveryTypeGetterService = deliveryTypeGetterService;
            _logger = logger;
            _accountService = accountService;   
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("OfferControler - GET Create Method called");
            ViewBag.HasAddress = await _accountService.DoesCurrentUserHaveAddress();
            var viewModel = new AddOfferViewModel();
            await _offerViewModelInitializer.InitializeAllAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidatePicturesFilter))]
        public async Task<IActionResult> Create(AddOfferViewModel viewModel)
        {
            _logger.LogInformation("OfferControler - POST Create Method called");
            var userHasAddress = await _accountService.DoesCurrentUserHaveAddress();
            ViewBag.HasAddress = userHasAddress;

            if (!userHasAddress)
            {
                ModelState.AddModelError(string.Empty, "You must add your address before creating an offer.");
                await _offerViewModelInitializer.InitializeAllAsync(viewModel);
                return View(viewModel);
            }

            if (!ModelState.IsValid)
            {
                await _offerViewModelInitializer.InitializeAllAsync(viewModel);

                _logger.LogWarning("Invalid Model state. Errors {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(item => item.Errors).Select(item => item.ErrorMessage)));

                return View(viewModel);
            }

            OfferAddRequest dto = viewModel.ToAddOfferDto();
            var result = await _offerService.Add(dto);

            if (result.IsFailure)
            {
                _logger.LogError("Error while adding offer for {UserName}. Error {Error}.",
                    User.Identity?.Name, result.Error.Description);

                return View("Error", result.Error.Description);
            }

            return RedirectToAction(nameof(UserOffers));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            _logger.LogInformation("OfferControler - GET Edit Method called with parameters {OfferId}", id);
            var response = await _offerService.GetOfferForEdit(id);

            if (response.IsFailure)
            {
                _logger.LogError("Error while fetching Offer for edit. Error: {Error}", response.Error.Description);
                return View("Error", response.Error.Description);
            }

            var viewModel = response.Value.ToEditOfferViewModel(_pictureHandlerService);
            await _offerViewModelInitializer.InitializeAllAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidatePicturesFilter))]
        public async Task<IActionResult> Edit(EditOfferViewModel viewModel)
        {
            _logger.LogInformation("POST Edit called by user {User} with view model {@ViewModel}", User.Identity?.Name, viewModel);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state. Errors: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(item => item.Errors).Select(item => item.ErrorMessage)));
                await _offerViewModelInitializer.InitializeAllAsync(viewModel);
                return View(viewModel);
            }

            var dto = viewModel.ToEditOfferDto();
            var result = await _offerService.Edit(dto);

            if (result.IsFailure)
            {
                _logger.LogError("Error while editing {UserName}'s offer. Error: {Error}",
                     User.Identity?.Name, result.Error.Description);

                return View("Error", result.Error.Description);
            }
               
            return RedirectToAction(nameof(UserOffers));
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation("OfferControler - POST Delete Method called with parameter: OfferId: {Param}", id);
            var result = await _offerService.DeleteOffer(id);

            if (result.IsFailure)
            {
                _logger.LogError("Error while deleting offer for {UserName}. Error: {Error}",
                     User.Identity?.Name, result.Error.Description);
                return Json(new JsonResponseModel() { Message = result.Error.Description, Success = false });
            }

            _logger.LogInformation("Successfully deleted offer of Id: {OfferId} for user {UserName}",
                id, User.Identity?.Name);
            return Json(new JsonResponseModel() { Message = "Deleted Offer Successfully !", Success = true });
        }

        [HttpGet]
        public async Task<IActionResult> UserOffers(string? title)
        {
            _logger.LogInformation("OfferControler - GET UserOffers Method called with parameters: Title: {Title}", title);
            IEnumerable<UserOfferResponse> response = await _offerService.GetFilteredUserOffers(title);
            IEnumerable<UserOffersViewModel> userOffers = response
                .Select(item => item.ToUserOffersViewModel(_pictureHandlerService));

            return View(userOffers);
        }

        [HttpGet]
        public async Task<IActionResult> FilterUserOffers(string? title)
        {
            _logger.LogInformation("OfferControler - GET FilterUserOffers Method called with parameters: Title: {Title}", title);
            IEnumerable<UserOfferResponse> response = await _offerService.GetFilteredUserOffers(title);
            IEnumerable<UserOffersViewModel> viewModel = response
                .Select(item => item.ToUserOffersViewModel(_pictureHandlerService));
            return PartialView("OfferPartials/_UserOfferListPartial", viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            _logger.LogInformation("OfferControler - GET Details Method called with parameters: OfferId: {OfferId}", id);
            var response = await _offerService.GetOffer(id);

            if (response.IsFailure)
            {
                _logger.LogError("Error while fetching offert to display. Error: {Error}", response.Error.Description);
                return View("Error", response.Error.Description);
            }
               
            var viewModel = response.Value.ToOfferDetailsViewModel(_pictureHandlerService);
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] OfferFilter filter)
        {
            _logger.LogInformation("OfferControler - GET Index Method called with parameters: {@OfferFilter}", filter);
            IEnumerable<OfferIndexResponse> filteredOffers = await _offerService.GetFilteredOffers(filter);
            OfferIndexViewModel viewModel = new OfferIndexViewModel()
            {
                Items = filteredOffers.Select(item => item.ToOfferIndexItemViewModel(_pictureHandlerService))
                    .ToList(),

                DeliveryOptions = (await _deliveryTypeGetterService.GetAllDeliveryTypesAsSelectionList())
                    .ToSelectListItem(),

                SortingOptions = _sortingOptionService.GetSortingOptions()
                    .ToSelectListItem(),

                Filter = filter,
            };
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FilterOffers([FromQuery] OfferFilter filter)
        {
            _logger.LogInformation("OfferControler - GET FilterOffers Method called with parameters: {@OfferFilter}", filter);
            IEnumerable<OfferIndexResponse> filteredOffers = await _offerService.GetFilteredOffers(filter);
            var viewModel = filteredOffers.Select(item => item.ToOfferIndexItemViewModel(_pictureHandlerService));
            return PartialView("OfferPartials/_OfferListPartial", viewModel);
        }
    }
}
