using CSOS.Core.DTO.AddressDto;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Helpers;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.AddressViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CSOS.UI.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<AddressController> _logger;
        public AddressController(IAddressService addressService, ICountriesGetterService countriesGetterService, ILogger<AddressController> logger)
        {
            _addressService = addressService;
            _countriesGetterService = countriesGetterService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddAddressViewModel viewModel)
        {
            var request = viewModel.ToAddressAddRequest();

            var result = await _addressService.AddAddress(request);

            if(result.IsFailure)
                return Json( new JsonResponseModel { Message = result.Error.Description, Success = false });

            var editResult = await _addressService.GetUserAddressForEdit();

            if (editResult.IsFailure)
                return Json(new JsonResponseModel { Message = "Address added, but failed to load edit form", Success = false });

            var editViewModel = editResult.Value.ToEditAddressViewModel();
            editViewModel.CountriesSelectionList = (await _countriesGetterService.GetCountriesSelectionList()).ToSelectListItem();

            return PartialView("AccountPartials/_AddressChangeForm", editViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            _logger.LogInformation("AddressController - GET Edit Method called with ID: {Id}", id);
            var result = await _addressService.GetUserAddressForEdit();

            if (result.IsFailure)
            {
                _logger.LogError("Failed to fetch Address for {UserName}, Errors: {Error}",
                    User.Identity?.Name, result.Error.Description);

                return View("Error", result.Error.Description);
            }
                

            EditAddressViewModel viewModel = result.Value.ToEditAddressViewModel();
            viewModel.CountriesSelectionList = (await _countriesGetterService.GetCountriesSelectionList()).ToSelectListItem();
            return PartialView("_EditAddressPartial", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAddressViewModel viewModel)
        {
            _logger.LogInformation("AddressController - POST Edit Method called");
            if (!ModelState.IsValid)
            {
                viewModel.CountriesSelectionList = (await _countriesGetterService.GetCountriesSelectionList())
                    .ToSelectListItem();

                _logger.LogWarning("Invalid model state: {Errors}", string
                    .Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

                return viewModel.Source switch
                {
                    "AddOrder" => PartialView("_EditAddressPartial", viewModel),
                    "AccountDetails" => PartialView("AccountPartials/_AddressChangeForm", viewModel),
                    _ => Json(new JsonResponseModel() { Message = "Something went wrong", Success = false })
                };
            }

            AddressUpdateRequest updateRequest = viewModel.ToAddressUpdateRequest();
            var result = await _addressService.EditUserAddress(updateRequest);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to edit address for {UserName}, Error: {Error}", User.Identity?.Name, result.Error.Description);
                return Json(new JsonResponseModel() { Message = result.Error.Description, Success = false });
            }
                
            _logger.LogInformation("Successfully edited address for {UserName}", User.Identity!.Name);
            return Json(new JsonResponseModel() { Message = "Address updated successfully !", Success = true });
        }

    }
}   
