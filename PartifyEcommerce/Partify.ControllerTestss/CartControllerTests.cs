using AutoFixture;
using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.CartDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Controllers;
using CSOS.UI.Helpers;
using CSOS.UI.Mappings.ToViewModel;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace CSOS.Tests
{
    public class CartControllerTests
    {
        private readonly ICartService _cartService;
        private CartController _cartController = null!;
        private readonly IConfigurationReader _configurationReader;
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly Mock<IPictureHandlerService> _pictureHandlerServiceMock;
        private readonly Mock<IConfigurationReader> _configurationReaderMock;
        private readonly IFixture _fixture;
        private readonly ILogger<CartController> _logger;

        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _configurationReaderMock = new Mock<IConfigurationReader>();
            _pictureHandlerServiceMock = new Mock<IPictureHandlerService>();
            _cartService = _cartServiceMock.Object;
            _configurationReader = _configurationReaderMock.Object;
            _logger = Mock.Of<ILogger<CartController>>();
            _pictureHandlerService = _pictureHandlerServiceMock.Object;
            _fixture = new Fixture();
        }

        public CartController CreateController()
        {
            var controller = new CartController(_cartService, _pictureHandlerService, _logger);

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

        #region Index Method Tests
        [Fact]
        public async Task Cart_CartResultFailure_ReturnsErrorView()
        {
            //Arrange
            _cartController = CreateController();
            _cartServiceMock.Setup(item => item.GetLoggedUserCart())
                .ReturnsAsync(Result.Failure<CartResponseDto>(CartErrors.CartDoesNotExists));

            //Act
            IActionResult result = await _cartController.Index();

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewData.Model.Should().Be(CartErrors.CartDoesNotExists.Description);
        }

        [Fact]
        public async Task Cart_CartResultSuccess_ReturnsView()
        {
            //Arrange
            _cartController = CreateController();
            CartResponseDto cartResponseDto =
                _fixture.Build<CartResponseDto>().Without(item => item.CartItems).Create();
            _cartServiceMock.Setup(item => item.GetLoggedUserCart()).ReturnsAsync(Result.Success(cartResponseDto));

            //Act
            IActionResult result = await _cartController.Index();

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewData.Model.Should().BeEquivalentTo(cartResponseDto.ToCartViewModel(_pictureHandlerService));
        }

        #endregion

        #region AddToCart Method Tests

        [Fact]
        public async Task AddToCart_AddingToCartFailure_ReturnsErrorJson()
        {
            //Arrange
            _cartController = CreateController();
            var error = OfferErrors.OfferDoesNotExist;
            _cartServiceMock.Setup(item => item.AddToCart(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Result.Failure(error));

            //Act
            IActionResult result = await _cartController.AddToCart(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel
                {
                    Success = false,
                    Message = $"Error: {OfferErrors.OfferDoesNotExist.Description}"
                });
        }

        [Fact]
        public async Task AddToCart_AddingToCartSuccess_ReturnsSuccessJson()
        {
            //Arrange
            _cartController = CreateController();
            _cartServiceMock.Setup(item => item.AddToCart(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Result.Success);

            //Act
            IActionResult result = await _cartController.AddToCart(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel
                {
                    Success = true,
                    Message = "Item Successfully Added to Cart",
                });
        }

        #endregion

        #region DeleteFromCart Method Tests
        
        [Fact]
        public async Task DeleteFromCart_DeleteFromCartFailure_ReturnsErrorJson()
        {
            //Arrange
            _cartController = CreateController();
            var error = CartItemErrors.CartItemDoesNotExists;
            _cartServiceMock.Setup(item => item.DeleteFromCart(It.IsAny<int>())).ReturnsAsync(Result.Failure(error));

            //Act
            IActionResult result = await _cartController.DeleteFromCart(It.IsAny<int>());

            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel()
                {
                    Success = false,
                    Message = $"Error: {error.Description}"
                });
        }

        [Fact]
        public async Task DeleteFromCart_DeleteFromCartSuccess_ReturnsSuccessJson()
        {
            //Arrange
            _cartController = CreateController();
            _cartServiceMock.Setup(item => item.DeleteFromCart(It.IsAny<int>())).ReturnsAsync(Result.Success);
            
            //Act
            IActionResult result = await _cartController.DeleteFromCart(It.IsAny<int>());
            
            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel()
                {
                    Success = true,
                    Message = "Item removed from cart successfully!",
                });
        }
        #endregion

        #region UpdateQuantityInCart Method Tests
        [Fact]
        public async Task UpdateQuantityInCart_UpdateCartItemQuantityFailure_ReturnErrorJson()
        {
            //Arrange
            _cartController = CreateController();
            var error = CartItemErrors.CartItemDoesNotExists;
            _cartServiceMock.Setup(item => item.UpdateCartItemQuantity(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(Result.Failure(error));

            //Act
            IActionResult result = await _cartController.UpdateQuantityInCart(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel()
                {
                    Success = false,
                    Message = $"Error: {error.Description}",
                });
        }

        [Fact]
        public async Task UpdateQuantityInCart_UpdateCartItemQuantitySuccess_ReturnSuccessJson()
        {
            //Arrange
            _cartController = CreateController();
            _cartServiceMock.Setup(item => item.UpdateCartItemQuantity(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(Result.Success);

            //Act
            IActionResult result = await _cartController.UpdateQuantityInCart(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel()
                {
                    Success = true,
                    Message = "Updated cart successfully!"
                });
        }
        #endregion

        #region GetCartItemsCount Method Tests
        [Fact]
        public void GetCartItemsCount_ReturnViewComponent()
        {
            //Act
            _cartController = CreateController();
            IActionResult result = _cartController.GetCartItemsCount();

            //Assert
            result.Should().BeOfType<ViewComponentResult>();
        }
        #endregion
    }
}