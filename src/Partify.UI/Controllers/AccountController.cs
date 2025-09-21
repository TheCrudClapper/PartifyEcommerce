using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.DTO.AccountDto;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Helpers;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.AccountViewModels;
using CSOS.UI.ViewModels.AddressViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ICountriesGetterService countriesGetterService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _countriesGetterService = countriesGetterService;
            _logger = logger;
        }


        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Register()
        {
            _logger.LogInformation("AccountController - GET Register method called.");
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            _logger.LogInformation("AccountController - POST Register method called for {Email}", registerRequest.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state during registration for {Email}. Errors: {Errors}",
                registerRequest.Email,string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

                return View(registerRequest);
            }

            IdentityResult result = await _accountService.Register(registerRequest);

            if (!result.Succeeded)
            {
                _logger.LogError("Registration failed for {Email}. Errors: {Errors}",
                   registerRequest.Email,
                   string.Join(", ", result.Errors.Select(e => e.Description)));

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerRequest);

            }
            _logger.LogInformation("User {Email} registered successfully", registerRequest.Email);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Login()
        {
            _logger.LogInformation("AccountController - GET Login method called.");
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Login(LoginRequest request, string? returnUrl)
        {
            _logger.LogInformation("POST Login Method called at time {Time}", DateTime.UtcNow);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state during login for {Email}. Errors: {Errors}",
                   request.Email,
                   string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

                return View(request);
            }

            var result = await _accountService.Login(request);
            if (!result.Succeeded)
            {
                _logger.LogError("Login failed for {Email}.", request.Email);
                ModelState.AddModelError("", "Invalid email or password");
                return View(request);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                _logger.LogInformation("User {Email} logged in successfully. Redirecting to {ReturnUrl}", request.Email, returnUrl); ;
                return LocalRedirect(returnUrl);
            }

            if(User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home", new { area = "Admin"});

            _logger.LogInformation("User {Email} logged in successfully.", request.Email);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            _logger.LogInformation("User {UserName} logged out.", User.Identity?.Name);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AccountDetails()
        {
            _logger.LogInformation("AccountController - GET AccountDetails method called for {UserName}", User.Identity?.Name);
            AccountDetailsViewModel viewModel = new AccountDetailsViewModel();
            var userHasAddress = await _accountService.DoesCurrentUserHaveAddress();
            var countries = (await _countriesGetterService.GetCountriesSelectionList()).ToSelectListItem();

            if (userHasAddress)
            {
                var accountDetailsResponse = await _accountService.GetAccountDetailsAsync();
                if (accountDetailsResponse.IsFailure)
                {
                    _logger.LogError("Failed to fetch account details for {UserName}. Error: {Error}",
                        User.Identity?.Name, accountDetailsResponse.Error.Description);

                    return View("Error", accountDetailsResponse.Error.Description);
                }
                viewModel.EditAddress = accountDetailsResponse.Value.AddressResponse.ToEditAddressViewModel();
                viewModel.EditAddress.CountriesSelectionList = countries;
                viewModel.UserDetails = accountDetailsResponse.Value.AccountResponse.ToUserDetailsViewModel();
            }
            else
            {
                var accountResponse = await _accountService.GetAccount();
                if (accountResponse.IsFailure)
                    return View("Error", accountResponse.Error.Description);

                viewModel.UserDetails = accountResponse.Value.ToUserDetailsViewModel();
                viewModel.AddAddressViewModel = new AddAddressViewModel();
                viewModel.AddAddressViewModel.CountriesSelectionList = countries;

            }

            _logger.LogInformation("Account details returned successfully for {UserName}", User.Identity?.Name ?? "");
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDetailsViewModel viewModel)
        {
            _logger.LogInformation("AccountController - POST Edit method called for {UserName}", User.Identity?.Name);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state during account edit for {UserName}. Errors: {Errors}",
                  User.Identity?.Name,
                  string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

                return PartialView("AccountPartials/_UserDetailsForm", viewModel);
            }

            AccountUpdateRequest request = viewModel.ToAccountUpdateRequest();
            var result = await _accountService.Edit(request);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to update user details for {UserName}. Error: {Error}", User.Identity?.Name, result.Error.Description);
                return Json(new JsonResponseModel() { Success = false, Message = $"Error: {result.Error.Description}" });
            }

            _logger.LogInformation("User details updated successfully for {UserName}", User.Identity?.Name);
            return Json(new JsonResponseModel() { Success = true, Message = "User details updated successfully !" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeRequest request)
        {
            _logger.LogInformation("AccountController - POST ChangePassword method called for {UserName}", User.Identity?.Name);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state during password change for {UserName}. Errors: {Errors}",
                   User.Identity?.Name,
                   string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

                return PartialView("AccountPartials/_PasswordChangeForm", request);
            }

            var result = await _accountService.ChangePassword(request);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to change password for {UserName}. Error: {Error}", User.Identity?.Name, result.Error.Description);
                return Json(new JsonResponseModel() { Success = false, Message = $"Error: {result.Error.Description}" });
            }

            _logger.LogInformation("Password changed successfully for {UserName}", User.Identity?.Name);
            return Json(new JsonResponseModel() { Success = true, Message = "Password changed successfully !" });
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("Error", "Access for this resource is denied");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyTaken(string email)
        {
            var exists = await _accountService.IsEmailAlreadyTaken(email);
            return Json(!exists);
        }
    }
}
