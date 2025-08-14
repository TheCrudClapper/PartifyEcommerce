using AutoFixture;
using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Controllers;
using CSOS.UI.ViewModels.HomePageViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CSOS.Tests
{
    public class HomeControllerTests
    {
        private readonly IOfferService _offerService;
        private readonly Mock<IOfferService> _offerServiceMock;
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly Mock<ICategoryGetterService> _categoryGetterServiceMock;
        private readonly IConfigurationReader _configurationReader;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly Mock<IPictureHandlerService> _pictureHandlerServiceMock;
        private readonly Mock<IConfigurationReader> _configuartionReaderMock;
        private HomeController _homeController = null!;
        private readonly IFixture _fixture;
        private readonly ILogger<HomeController> _logger;
        public HomeControllerTests()
        {
            _fixture = new Fixture();
            _offerServiceMock = new Mock<IOfferService>();
            _categoryGetterServiceMock = new Mock<ICategoryGetterService>();
            _configuartionReaderMock = new Mock<IConfigurationReader>();
            _pictureHandlerServiceMock = new Mock<IPictureHandlerService>();
            _categoryGetterService = _categoryGetterServiceMock.Object;
            _offerService = _offerServiceMock.Object;
            _configurationReader = _configuartionReaderMock.Object;
            _pictureHandlerService = _pictureHandlerServiceMock.Object;
            _logger = Mock.Of<ILogger<HomeController>>();
        }
        private HomeController CreateController()
        {
            return new HomeController(_offerService, _categoryGetterService, _logger, _pictureHandlerService);
        }
        #region Index Method Tests
        [Fact]
        public async Task Index_ReturnViewsWithCorrectViewModel()
        {
            //Arrange
            IEnumerable<CardResponse> offers = _fixture.CreateMany<CardResponse>();
            IEnumerable<SelectListItemDto> categories = _fixture.CreateMany<SelectListItemDto>();

            _offerServiceMock.Setup(item => item.GetIndexPageOffers()).ReturnsAsync(offers);

            _categoryGetterServiceMock.Setup(item => item.GetProductCategoriesAsSelectList()).ReturnsAsync(categories);

            _categoryGetterServiceMock.Setup(item => item.GetProductCategoriesAsCardResponse()).ReturnsAsync(_fixture.CreateMany<CardResponse>());

            _offerServiceMock.Setup(item => item.GetDealsOfTheDay()).ReturnsAsync(offers);

            _homeController = CreateController();

            //Act
            var result = await _homeController.Index();

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<IndexPageViewModel>().Subject;

            model.Cards.Should().NotBeNull();
            model.Cards.Should().HaveCount(offers.Count());
            model.BestDeals.Should().NotBeNull();
            model.BestDeals.Should().HaveCount(offers.Count());
            model.Categories.Should().NotBeNull();
            model.Categories.Should().HaveCount(categories.Count());
            model.CategoriesSlider.Should().NotBeNull();
            model.CategoriesSlider.Should().HaveCount(3);
        }
        #endregion

        #region Privacy Method Tests
        [Fact]
        public void Privacy_ReturnsView()
        {
            //Arrange
            _homeController = CreateController();

            //Act
            IActionResult result = _homeController.Privacy();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }
        #endregion

        #region AboutUs Method Tests
        [Fact]
        public void AboutUs_ReturnsView()
        {
            //Arrange
            _homeController  = CreateController();

            //Act
            IActionResult result = _homeController.AboutUs();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }
        #endregion

        #region Error Method Tests
        [Fact]
        public void Error_WithExceptionHandler_SetsViewBagError()
        {
            // Arrange
            var _homeController = CreateController();

            var exceptionFeature = new Mock<IExceptionHandlerPathFeature>();
            exceptionFeature.Setup(f => f.Error).Returns(new Exception("Test exception message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Features.Set(exceptionFeature.Object);

            _homeController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = _homeController.Error();

            // Assert
            Assert.Equal("Test exception message", _homeController.ViewBag.Error);
        }

        [Fact]
        public void Error_WithoutExceptionHandler_ReturnsViewWithoutError()
        {
            // Arrange
            var _homeController = CreateController();
            _homeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() 
            };

            // Act
            var result = _homeController.Error();

            // Assert
            result.Should().BeOfType<ViewResult>();
            Assert.Null(_homeController.ViewBag.Error);
        }
        #endregion
    }
}
