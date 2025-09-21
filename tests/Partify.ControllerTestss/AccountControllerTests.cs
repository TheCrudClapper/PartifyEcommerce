using Moq;
using AutoFixture;
using CSOS.Core.DTO.AccountDto;
using CSOS.Core.DTO.UniversalDto;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using CSOS.Core.ResultTypes;
using CSOS.UI.ViewModels.AccountViewModels;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Controllers;
using CSOS.UI.Helpers;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CSOS.Tests
{
    public class AccountControllerTests
    {
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<ICountriesGetterService> _countriesGetterServiceMock;
        private readonly Mock<IAddressService> _addressServiceMock;
        private readonly ILogger<AccountController> _logger;
        private readonly IFixture _fixture;
        private AccountController _accountController = null!;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _countriesGetterServiceMock = new Mock<ICountriesGetterService>();
            _addressServiceMock = new Mock<IAddressService>();
            _accountService = _accountServiceMock.Object;
            _countriesGetterService = _countriesGetterServiceMock.Object;
            _addressService = _addressServiceMock.Object;
            _logger = Mock.Of<ILogger<AccountController>>();
            _fixture = new Fixture();
        }

        private AccountController CreateController()
        {
            var controller = new AccountController(
                _accountServiceMock.Object,
                _countriesGetterServiceMock.Object,
                _logger
            );

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return controller;

        }

        #region Register GET Method Tests

        [Fact]
        public void Register_ReturnsViewWithViewModel()
        {
            //Arrange
            _accountController = CreateController();

            //Act
            IActionResult result = _accountController.Register();

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
        }

        #endregion

        #region Register POST Method Tests

        [Fact]
        public async Task Register_SuccededRegister_RedirectToIndex()
        {
            //Arrange
            _accountController = CreateController();
            RegisterRequest request = _fixture.Build<RegisterRequest>()
                .Create();

            _accountServiceMock.Setup(item => item.Register(It.IsAny<RegisterRequest>())).ReturnsAsync(IdentityResult.Success);

            _accountController = CreateController();

            //Act
            IActionResult result = await _accountController.Register(request);

            //Assert
            RedirectToActionResult redirect = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirect.ControllerName.Should().Be("Home");
            redirect.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Register_FailedRegister_ReturnView()
        {
            //Arrange
            _accountController = CreateController();
            RegisterRequest request = _fixture.Build<RegisterRequest>()
              .Create();


            _accountServiceMock.Setup(item => item.Register(It.IsAny<RegisterRequest>())).ReturnsAsync(IdentityResult.Failed());

            _accountController = CreateController();

            //Act
            IActionResult result = await _accountController.Register(request);

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeOfType<RegisterRequest>();
        }
        #endregion

        #region Login GET Method Tests
        [Fact]
        public void Login_ReturnsView()
        {
            //Arrange
            _accountController = CreateController();
            _accountController = CreateController();

            //Act
            IActionResult result = _accountController.Login();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        #endregion

        #region Login POST Method Tests
        [Fact]
        public async Task Login_InvalidModelState_ReturnsView()
        {
            //Arrange
            _accountController = CreateController();
            LoginRequest request = _fixture.Create<LoginRequest>();
            _accountController = CreateController();
            _accountController.ModelState.AddModelError("key1", "Test dummy error");

            //Act
            IActionResult result = await _accountController.Login(request, It.IsAny<string>());

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(request);
        }

        [Fact]
        public async Task Login_SuccededLogin_RedirectsToAction()
        {
            //Arrange
            _accountController = CreateController();
            LoginRequest request = _fixture.Create<LoginRequest>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Login(It.IsAny<LoginRequest>())).ReturnsAsync(SignInResult.Success);

            //Act
            IActionResult result = await _accountController.Login(request, string.Empty);

            //Arrange
            RedirectToActionResult redirectToActionResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Home");
        }

        [Fact]
        public async Task Login_ReturnUrlValid_ReturnsLocalRedirect()
        {
            //Arrange
            _accountController = CreateController();
            LoginRequest request = _fixture.Create<LoginRequest>();

            string localUrl = "~/Offer/Index";

            _accountController = CreateController();

            _accountServiceMock.Setup(item => item.Login(It.IsAny<LoginRequest>())).ReturnsAsync(SignInResult.Success);

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(item => item.IsLocalUrl(localUrl)).Returns(true);

            _accountController.Url = urlHelperMock.Object;

            //Act
            IActionResult result = await _accountController.Login(request, localUrl);

            //Assert
            LocalRedirectResult localRedirectResult = result.Should().BeOfType<LocalRedirectResult>().Subject;
            localRedirectResult.Url.Should().Be(localUrl);
        }

        [Fact]
        public async Task Login_FailedLogin_ReturnsView()
        {
            //Arrange
            _accountController = CreateController();
            LoginRequest request = _fixture.Create<LoginRequest>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Login(It.IsAny<LoginRequest>())).ReturnsAsync(SignInResult.Failed);

            //Act
            IActionResult result = await _accountController.Login(request, string.Empty);

            //Arrange
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(request);
            _accountController.ModelState.IsValid.Should().BeFalse();
            _accountController.ModelState[""]!.Errors.Should().NotBeNull();
        }
        #endregion

        #region Logout Method Tests
        [Fact]
        public async Task Logout_RedirectsToAction()
        {
            //Arrange
            _accountController = CreateController();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Logout()).Returns(Task.CompletedTask);

            //Act
            IActionResult result = await _accountController.Logout();

            //Assert
            RedirectToActionResult redirectToActionResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Home");
        }
        #endregion

        #region AccountDetails GET Method Tests
        [Fact]
        public async Task AccountDetails_FailureServiceResult_ReturnsErrorView()
        {
            //Arrange
            _accountController = CreateController();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.DoesCurrentUserHaveAddress()).ReturnsAsync(true);
            _accountServiceMock.Setup(item => item.GetAccountDetailsAsync()).ReturnsAsync(Result.Failure<AccountDetailsResponse>(AddressErrors.AddressNotFound));

            //Act
            IActionResult result = await _accountController.AccountDetails();

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewName.Should().Be("Error");
            var viewModel = viewResult.Model.Should().BeOfType<string>();
            viewModel.Subject.Should().BeSameAs(AddressErrors.AddressNotFound.Description);
        }


        //To be repiared and the whole edit addres mess fixed
        [Fact]
        public async Task AccountDetails_SuccessServiceResult_ReturnsView()
        {
            //Arrange
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.DoesCurrentUserHaveAddress()).ReturnsAsync(true);
            _accountController = CreateController();
            AccountDetailsResponse accountDetailsResponse = _fixture.Create<AccountDetailsResponse>();

            AccountDetailsViewModel expectedViewModel = new AccountDetailsViewModel()
            {
                EditAddress = accountDetailsResponse.AddressResponse.ToEditAddressViewModel(),
                UserDetails = accountDetailsResponse.AccountResponse.ToUserDetailsViewModel(),
            };

            IEnumerable<SelectListItemDto> countries = _fixture.CreateMany<SelectListItemDto>();

            expectedViewModel.EditAddress.CountriesSelectionList = countries.ToSelectListItem();

            _accountServiceMock.Setup(item => item.GetAccountDetailsAsync()).ReturnsAsync(Result.Success(accountDetailsResponse));

            //Act
            IActionResult result = await _accountController.AccountDetails();

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var viewModel = viewResult.Model.Should().BeOfType<AccountDetailsViewModel>();
            //Assert wheret the second item is right
            viewModel.Subject.UserDetails.Should().BeEquivalentTo(expectedViewModel.UserDetails);
        }
        #endregion

        #region EditUserAddress POST Method Tests
        [Fact]
        public async Task Edit_InvalidModelState_ReturnsPartial()
        {
            //Arrange
            _accountController = CreateController();
            UserDetailsViewModel viewModel = _fixture.Create<UserDetailsViewModel>();
            _accountController = CreateController();
            _accountController.ModelState.AddModelError("test", "test");

            //Act
            IActionResult result = await _accountController.Edit(viewModel);

            //Arrange
            PartialViewResult partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            partialViewResult.Model.Should().BeEquivalentTo(viewModel);
        }

        [Fact]
        public async Task Edit_FailureServiceResult_ReturnsErrorJson()
        {
            //Arrange
            _accountController = CreateController();
            UserDetailsViewModel viewModel = _fixture.Create<UserDetailsViewModel>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Edit(It.IsAny<AccountUpdateRequest>())).ReturnsAsync(Result.Failure(AccountErrors.AccountNotFound));

            //Act
            IActionResult result = await _accountController.Edit(viewModel);

            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            var responseModel = jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject;
            responseModel.Success.Should().BeFalse();
            responseModel.Message.Should().Be($"Error: {AccountErrors.AccountNotFound.Description}");
        }

        [Fact]
        public async Task Edit_SuccessServiceResult_ReturnsErrorJson()
        {
            //Arrange
            _accountController = CreateController();
            UserDetailsViewModel viewModel = _fixture.Create<UserDetailsViewModel>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Edit(It.IsAny<AccountUpdateRequest>())).ReturnsAsync(Result.Success);

            //Act
            IActionResult result = await _accountController.Edit(viewModel);

            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            var responseModel = jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject;
            responseModel.Success.Should().BeTrue();
            responseModel.Message.Should().Be("User details updated successfully !");
        }
        #endregion

        #region ChangePassword POST Method Tests

        [Fact]
        public async Task ChangePassword_InvalidModelState_ReturnsPartial()
        {
            //Arrange
            _accountController = CreateController();
            PasswordChangeRequest viewModel = _fixture.Create<PasswordChangeRequest>();
            _accountController = CreateController();
            _accountController.ModelState.AddModelError("test", "test");

            //Act
            IActionResult result = await _accountController.ChangePassword(viewModel);

            //Assert
            PartialViewResult partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            partialViewResult.Model.Should().BeEquivalentTo(viewModel);
        }

        [Fact]
        public async Task ChangePassword_FailureServiceResult_ReturnsErrorJson()
        {
            //Arrange
            _accountController = CreateController();
            PasswordChangeRequest viewModel = _fixture.Create<PasswordChangeRequest>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.ChangePassword(It.IsAny<PasswordChangeRequest>()))
                .ReturnsAsync(Result.Failure(AccountErrors.PasswordChangeFailed));

            //Act
            IActionResult result = await _accountController.ChangePassword(viewModel);

            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            var responseModel = jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject;
            responseModel.Success.Should().BeFalse();
            responseModel.Message.Should().Be($"Error: {AccountErrors.PasswordChangeFailed.Description}");
        }

        [Fact]
        public async Task ChangePassword_SuccessServiceResult_ReturnsErrorJson()
        {
            //Arrange
            _accountController = CreateController();
            PasswordChangeRequest viewModel = _fixture.Create<PasswordChangeRequest>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.ChangePassword(It.IsAny<PasswordChangeRequest>()))
                .ReturnsAsync(Result.Success);

            //Act
            IActionResult result = await _accountController.ChangePassword(viewModel);

            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            var responseModel = jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject;
            responseModel.Success.Should().BeTrue();
            responseModel.Message.Should().Be("Password changed successfully !");
        }
        #endregion
    }
}
