using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IAddressService addressService, IAccountService accountService, ILogger<OrderController> logger)
        {
            _addressService = addressService;
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> AddOrder()
        {
            _logger.LogInformation("Order Controller - GET AddOrder Method Called");
            var userHasAddress = await _accountService.DoesCurrentUserHaveAddress();
            var viewModel = new AddOrderViewModel();

            if (userHasAddress)
            {
                var response = await _addressService.GetUserAddressDetails();

                if (response.IsFailure)
                {
                    _logger.LogError("Error while fetching {UserName} address. Error {Error}.",
                        User.Identity?.Name, response.Error.Description);
                    return View("Error", response.Error.Description);
                }

                viewModel.UserAddressDetails = response.Value;
            }

            return View(viewModel);
        }
    }
}
