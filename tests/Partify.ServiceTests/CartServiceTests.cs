using AutoFixture;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Services;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.CartDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.ServiceContracts;
using CSOS.Core.Services;
using FluentAssertions;
using Moq;
using Xunit.Abstractions;
namespace CSOS.Tests
{
    public class CartServiceTests
    {
        private readonly ICartService _cartService;
        private readonly ICartRepository _cartRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITestOutputHelper _testHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IOfferRepository> _offerRepositoryMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly IFixture _fixture;
        public CartServiceTests(ITestOutputHelper testHelper)
        {
            //should be mocked
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _offerRepositoryMock = new Mock<IOfferRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _cartRepository = _cartRepositoryMock.Object;
            _unitOfWork = _unitOfWorkMock.Object;
            _currentUserService = _currentUserServiceMock.Object;
            _offerRepository = _offerRepositoryMock.Object;
            _testHelper = testHelper;
            _cartService = new CartService(_currentUserService, _offerRepository, _cartRepository, _unitOfWork);
            _fixture = new Fixture();

            _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        #region AddToCart Method Tests
        [Fact]
        public async Task AddToCart_QuantityLowerThanZero_ReturnsFailureResult()
        {
            //Arrange
            int quantity = -1;
            int offerId = _fixture.Create<int>();

            //Act
            var result = await _cartService.AddToCart(offerId, quantity);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartItemErrors.QuantityLowerThanZero);
        }

        [Fact]
        public async Task AddToCart_OfferNull_ReturnsFailureResult()
        {
            //Arrange
            int quantity = _fixture.Create<int>();
            int offerId = _fixture.Create<int>();
            _offerRepositoryMock.Setup(item => item.GetOfferByIdAsync(It.IsAny<int>())).ReturnsAsync((Offer?)null);

            //Act
            var result = await _cartService.AddToCart(offerId, quantity);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(OfferErrors.OfferDoesNotExist);
        }

        [Fact]
        public async Task AddToCart_CartIdNull_ReturnsFailureResult()
        {
            //Arrange
            int quantity = _fixture.Create<int>();
            int offerId = _fixture.Create<int>();
            Offer offer = _fixture.Create<Offer>();

            _offerRepositoryMock.Setup(item => item.GetOfferByIdAsync(It.IsAny<int>())).ReturnsAsync(offer);
            _cartRepositoryMock.Setup(item => item.GetLoggedUserCartIdAsync(It.IsAny<Guid>())).ReturnsAsync((int?)null);

            //Act
            var result = await _cartService.AddToCart(offerId, quantity);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartErrors.CartDoesNotExists);
        }

        [Fact]
        public async Task AddToCart_ItemExistInCartAndQuantitySumIsLowerOrSameAsStockQuantity_UpdatesItemQuantity()
        {
            //Arrange
            int quantityToAdd = 14;

            int offerId = _fixture.Create<int>();

            int cartId = _fixture.Create<int>();

            int stockQuantity = 20;

            int currentCartItemQuantity = 5;

            CartItem cartItem = _fixture.Build<CartItem>()
                .With(item => item.CartId, cartId)
                .With(item => item.OfferId, offerId)
                .With(item => item.Quantity, currentCartItemQuantity)
                .Create();

            Offer offer = _fixture.Build<Offer>()
                .With(item => item.Id, offerId)
                .With(item => item.StockQuantity, stockQuantity)
                .With(item => item.CartItems, new List<CartItem>() { cartItem })
                .Create();

            _offerRepositoryMock.Setup(item => item.GetOfferByIdAsync(It.IsAny<int>())).ReturnsAsync(offer);
            _cartRepositoryMock.Setup(item => item.GetLoggedUserCartIdAsync(It.IsAny<Guid>())).ReturnsAsync(cartId);
            _cartRepositoryMock.Setup(item => item.GetCartItemAsync(cartId, offer.Id)).ReturnsAsync(cartItem);

            //Act
            var result = await _cartService.AddToCart(offerId, quantityToAdd);

            //Assert
            result.IsSuccess.Should().BeTrue();
            cartItem.Quantity.Should().Be(quantityToAdd + currentCartItemQuantity);
            cartItem.DateCreated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public async Task AddToCart_ItemExistInCartAndQuantitySumIsHigherThanStockQuantity_ReturnsFailureResult()
        {
            //Arrange
            int quantityToAdd = 24;

            int offerId = _fixture.Create<int>();

            int cartId = _fixture.Create<int>();

            int stockQuantity = 20;

            int currentCartItemQuantity = 5;

            CartItem cartItem = _fixture.Build<CartItem>()
                .With(item => item.CartId, cartId)
                .With(item => item.OfferId, offerId)
                .With(item => item.Quantity, currentCartItemQuantity)
                .Create();

            Offer offer = _fixture.Build<Offer>()
                .With(item => item.Id, offerId)
                .With(item => item.StockQuantity, stockQuantity)
                .With(item => item.CartItems, new List<CartItem>() { cartItem })
                .Create();

            _offerRepositoryMock.Setup(item => item.GetOfferByIdAsync(It.IsAny<int>())).ReturnsAsync(offer);
            _cartRepositoryMock.Setup(item => item.GetLoggedUserCartIdAsync(It.IsAny<Guid>())).ReturnsAsync(cartId);
            _cartRepositoryMock.Setup(item => item.GetCartItemAsync(cartId, offer.Id)).ReturnsAsync((CartItem?)null);

            //Act
            var result = await _cartService.AddToCart(offerId, quantityToAdd);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartItemErrors.CannotAddMoreToCart);
        }

        [Fact]
        public async Task AddToCart_ItemNotInCartQuantityHigherThanInStock_ReturnsFailureResult()
        {
            //Arrange
            int quantityToAdd = 22;

            int offerId = _fixture.Create<int>();

            int cartId = _fixture.Create<int>();

            int stockQuantity = 20;

            Offer offer = _fixture.Build<Offer>()
                .With(item => item.Id, offerId)
                .With(item => item.StockQuantity, stockQuantity)
                .Create();

            _offerRepositoryMock.Setup(item => item.GetOfferByIdAsync(It.IsAny<int>())).ReturnsAsync(offer);
            _cartRepositoryMock.Setup(item => item.GetLoggedUserCartIdAsync(It.IsAny<Guid>())).ReturnsAsync(cartId);
            _cartRepositoryMock.Setup(item => item.GetCartItemAsync(cartId, offer.Id)).ReturnsAsync((CartItem?)null);

            //Act
            var result = await _cartService.AddToCart(offerId, quantityToAdd);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartItemErrors.CannotAddMoreToCart);
        }

        [Fact]
        public async Task AddToCart_ItemNotInCartQuantityValid_AddsCartItem()
        {
            //Arrange
            int quantityToAdd = 18;

            int offerId = _fixture.Create<int>();

            int cartId = _fixture.Create<int>();

            int stockQuantity = 20;

            Offer offer = _fixture.Build<Offer>()
                .With(item => item.Id, offerId)
                .With(item => item.CartItems, new List<CartItem>())
                .With(item => item.StockQuantity, stockQuantity)
                .Create();

            _offerRepositoryMock.Setup(item => item.GetOfferByIdAsync(It.IsAny<int>())).ReturnsAsync(offer);
            _cartRepositoryMock.Setup(item => item.GetLoggedUserCartIdAsync(It.IsAny<Guid>())).ReturnsAsync(cartId);
            _cartRepositoryMock.Setup(item => item.GetCartItemAsync(cartId, offer.Id)).ReturnsAsync((CartItem?)null);
            _cartRepositoryMock.Setup(item => item.AddAsync(It.IsAny<CartItem>())).Returns(Task.CompletedTask);

            //Act
            var result = await _cartService.AddToCart(offerId, quantityToAdd);

            //Assert
            result.IsSuccess.Should().BeTrue();
            _cartRepositoryMock.Verify(repo =>
                repo.AddAsync(It.Is<CartItem>(ci =>
                    ci.Quantity == quantityToAdd &&
                    ci.Offer == offer &&
                    ci.CartId == cartId
                )), Times.Once);
        }
        #endregion


        #region UpdateCartItemQuantity Method Tests
        [Fact]
        public async Task UpdateCartItemQuantity_NullCartItem_ReturnsFailureResult()
        {
            //Arrange
            int invalidCartItemId = _fixture.Create<int>();
            int quantity = 1;
            _cartRepositoryMock.Setup(item => item.GetCartItemWithOfferAsync(invalidCartItemId)).ReturnsAsync((CartItem?)null);

            //Act
            var result = await _cartService.UpdateCartItemQuantity(invalidCartItemId, quantity);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartItemErrors.CartItemDoesNotExists);
        }

        [Fact]
        public async Task UpdateCartItemQuantity_OfferNotIncluded_ReturnsFailureResult()
        {
            //Arrange
            int validCartItemId = _fixture.Create<int>();
            int quantity = 1;
            CartItem cartItem = _fixture.Build<CartItem>()
                .With(item => item.Offer, null as Offer)
                .Create();

            _cartRepositoryMock.Setup(item => item.GetCartItemWithOfferAsync(validCartItemId)).ReturnsAsync(cartItem);

            //Act
            var result = await _cartService.UpdateCartItemQuantity(validCartItemId, quantity);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(OfferErrors.OfferIsNull);

        }

        [Fact]
        public async Task UpdateCartItemQuantity_QuantityLessThanZero_DeletesItemFromCart()
        {
            //Arrange
            int validCartItemId = _fixture.Create<int>();
            int quantity = -1;
            CartItem cartItem = _fixture.Build<CartItem>()
                .With(item => item.IsActive, true)
                .With(item => item.DateDeleted, null as DateTime?)
                .Create();

            _cartRepositoryMock.Setup(item => item.GetCartItemWithOfferAsync(validCartItemId)).ReturnsAsync(cartItem);
            _cartRepositoryMock.Setup(item => item.GetCartItemByIdAsync(validCartItemId)).ReturnsAsync(cartItem);

            //Act
            var result = await _cartService.UpdateCartItemQuantity(validCartItemId, quantity);

            //Assert
            result.IsSuccess.Should().BeTrue();
            cartItem.DateDeleted.Should().NotBeNull();
            cartItem.IsActive.Should().Be(false);
        }

        [Fact]
        public async Task UpdateCartItemQuantity_ValidQuantity_IncreasesCartItemQuantity()
        {
            //Arrange
            int validCartItemId = _fixture.Create<int>();
            int quantity = 10;
            Offer offer = _fixture.Build<Offer>().With(item => item.StockQuantity, 20).Create();

            CartItem cartItem = _fixture.Build<CartItem>()
                .With(item => item.IsActive, true)
                .With(item => item.Quantity, 1)
                .With(item => item.Offer, offer)
                .With(item => item.DateDeleted, null as DateTime?)
                .Create();

            _cartRepositoryMock.Setup(item => item.GetCartItemWithOfferAsync(validCartItemId)).ReturnsAsync(cartItem);

            //Act
            var result = await _cartService.UpdateCartItemQuantity(validCartItemId, quantity);

            //Assert
            result.IsSuccess.Should().BeTrue();
            cartItem.DateEdited.Should().NotBeNull();
            cartItem.Quantity.Should().Be(quantity);
        }
        [Fact]
        public async Task UpdateCartItemQuantity_QuantityBiggerThanInStock_SetsQuantityOfCartItemToGivenValue()
        {
            //Arrange
            int validCartItemId = _fixture.Create<int>();
            int quantity = 10;
            Offer offer = _fixture.Build<Offer>().With(item => item.StockQuantity, 9).Create();

            CartItem cartItem = _fixture.Build<CartItem>()
                .With(item => item.IsActive, true)
                .With(item => item.Quantity, 1)
                .With(item => item.Offer, offer)
                .With(item => item.DateDeleted, null as DateTime?)
                .Create();

            _cartRepositoryMock.Setup(item => item.GetCartItemWithOfferAsync(validCartItemId)).ReturnsAsync(cartItem);

            //Act
            var result = await _cartService.UpdateCartItemQuantity(validCartItemId, quantity);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartItemErrors.CannotAddMoreToCart);
            cartItem.DateEdited.Should().NotBeNull();
            cartItem.Quantity.Should().Be(offer.StockQuantity);
        }
        #endregion

        #region UpdateTotalCartValues Method Tests
        [Fact]
        public async Task UpdateTotalCartValues_NullCartItems_ReturnFailureResult()
        {
            //Arrange
            int invalidCartId = _fixture.Create<int>();
            _cartRepositoryMock.Setup(item => item.GetCartItemsForCostsUpdateAsync(invalidCartId)).ReturnsAsync((List<CartItem>?)null);

            //Act
            var result = await _cartService.UpdateTotalCartValue(invalidCartId);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartErrors.CartDoesNotExists);
        }

        [Fact]
        public async Task UpdateTotalCartValues_ZeroItemsInCartItemList_ReturnSuccessResult()
        {
            //Arrange
            int validCartId = _fixture.Create<int>();
            _cartRepositoryMock.Setup(item => item.GetCartItemsForCostsUpdateAsync(validCartId)).ReturnsAsync(new List<CartItem>() { });

            //Act
            var result = await _cartService.UpdateTotalCartValue(validCartId);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateTotalCartValues_CartIsNull_ReturnFailureResult()
        {
            //Arrange
            int validCartId = _fixture.Create<int>();
            IEnumerable<CartItem> cartItems = _fixture.CreateMany<CartItem>(4);


            _cartRepositoryMock.Setup(item => item.GetCartItemsForCostsUpdateAsync(validCartId)).ReturnsAsync(cartItems);
            _cartRepositoryMock.Setup(item => item.GetCartByIdAsync(validCartId)).ReturnsAsync((Cart?)null);

            //Act
            var result = await _cartService.UpdateTotalCartValue(validCartId);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartErrors.CartDoesNotExists);
        }

        [Fact]
        public async Task UpdateTotalCartValues_ValidDetails_ReturnSuccesUpdateValues()
        {
            //Arrange
            int validCartId = _fixture.Create<int>();

            _testHelper.WriteLine("Index Items:");
            IEnumerable<CartItem> cartItems = _fixture.Build<CartItem>()
                .With(item => item.Quantity, 10)
                .CreateMany(4);

            foreach (CartItem item in cartItems)
            {
                _testHelper.WriteLine(item.ToString() + "\n");
            }

            _testHelper.WriteLine("Index Values before updating costs:");
            //creating cart with given cart items 
            Cart cart = _fixture.Build<Cart>()
                .With(item => item.TotalCartValue, 0)
                .With(item => item.TotalItemsValue, 0)
                .With(item => item.MinimalDeliveryValue, 0)
                .Create();

            _testHelper.WriteLine($"Total Index Value: {cart.TotalCartValue}\nTotal Items Value: {cart.TotalItemsValue}\n, Minimal DeliveryTypeDto Value: {cart.MinimalDeliveryValue}\n");

            _cartRepositoryMock.Setup(item => item.GetCartItemsForCostsUpdateAsync(validCartId)).ReturnsAsync(cartItems);
            _cartRepositoryMock.Setup(item => item.GetCartByIdAsync(validCartId)).ReturnsAsync(cart);
            _unitOfWorkMock.Setup(item => item.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

            //Act
            var result = await _cartService.UpdateTotalCartValue(validCartId);

            //Assert
            result.IsSuccess.Should().BeTrue();
            cart.TotalItemsValue.Should().NotBe(0);
            cart.TotalCartValue.Should().NotBe(0);
            cart.MinimalDeliveryValue.Should().NotBe(0);

            _testHelper.WriteLine("Index Values after updating costs:");
            _testHelper.WriteLine($"Total Index Value: {cart.TotalCartValue}\nTotal Items Value: {cart.TotalItemsValue}\nMinimal DeliveryTypeDto Value: {cart.MinimalDeliveryValue}");
        }
        #endregion 

        #region GetLoggedUserCart Method Tests
        [Fact]
        public async Task GetLoggedUserCart_InvalidUserId_ReturnFailureResult()
        {
            //Arrange
            var userId = Guid.NewGuid();

            //_currentUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);
            _cartRepositoryMock.Setup(x => x.GetLoggedUserCartIdAsync(userId)).ReturnsAsync((int?)null);

            //Act
            Result result = await _cartService.GetLoggedUserCart();

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartErrors.CartDoesNotExists);
        }

        [Fact]
        public async Task GetLoggedUserCart_ValidDetails_ReturnCartResponseDto()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var cartId = _fixture.Create<int>();

            //Creating cart items
            var cartItems = _fixture.Build<CartItem>()
                .With(item => item.CartId, cartId)
                .With(item => item.IsActive, true)
                .CreateMany(3)
                .ToList();

            //Creating cart
            var cart = _fixture.Build<Cart>()
                .With(item => item.Id, cartId)
                .With(item => item.IsActive, true)
                .With(item => item.UserId, userId)
                .With(item => item.CartItems, cartItems)
                .Create();

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);
            _cartRepositoryMock.Setup(item => item.GetLoggedUserCartIdAsync(userId)).ReturnsAsync(cartId);
            _cartRepositoryMock.Setup(item => item.GetCartWithAllDetailsAsync(cartId)).ReturnsAsync(cart);

            //Act
            Result<CartResponseDto> result = await _cartService.GetLoggedUserCart();

            //Converting actual response to dto
            CartResponseDto expectedDto = cart.ToCartResponseDto();

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.CartItems.Should().HaveCount(cartItems.Count);
            result.Value.Should().BeEquivalentTo(expectedDto);

            //Compare a few general properties
            result.Value.TotalCartValue.Should().Be(cart.TotalCartValue);
            result.Value.TotalItemsValue.Should().Be(cart.TotalItemsValue);
            result.Value.TotalDeliveryValue.Should().Be(cart.MinimalDeliveryValue);
        }

        [Fact]
        public async Task GetLoggedUserCart_InvalidCartId_ReturnFailureResult()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var cartId = _fixture.Create<int>();

            _currentUserServiceMock.Setup(item => item.GetUserId()).Returns(userId);
            _cartRepositoryMock.Setup(item => item.GetLoggedUserCartIdAsync(userId)).ReturnsAsync(cartId);
            _cartRepositoryMock.Setup(item => item.GetCartWithAllDetailsAsync(cartId)).ReturnsAsync((Cart?)null);

            //Act
            Result result = await _cartService.GetLoggedUserCart();

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartErrors.CartDoesNotExist(cartId));
        }
        #endregion

        #region GetLoggedUserCartId Method Tests
        [Fact]
        public async Task GetLoggedUserCartId_InvalidUserGuid_ReturnsFailureResult()
        {
            // Arrange
            var invalidUserId = Guid.NewGuid();

            _currentUserServiceMock.Setup(x => x.GetUserId())
                .Returns(invalidUserId);

            _cartRepositoryMock.Setup(x => x.GetLoggedUserCartIdAsync(invalidUserId))
                .ReturnsAsync((int?)null);

            // Act
            var result = await _cartService.GetLoggedUserCartId();

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartErrors.CartDoesNotExists);
        }

        [Fact]
        public async Task GetLoggedUserCartId_ValidUserId_ReturnsValidCartId()
        {
            //Arrange
            var validUserId = Guid.NewGuid();

            var cartId = _fixture.Create<int>();

            _currentUserServiceMock.Setup(x => x.GetUserId())
                .Returns(validUserId);

            _cartRepositoryMock.Setup(x => x.GetLoggedUserCartIdAsync(validUserId))
                .ReturnsAsync(cartId);

            //Act
            var actual = await _cartService.GetLoggedUserCartId();

            //Assert
            actual.Value.Should().Be(cartId);
            actual.IsSuccess.Should().BeTrue();
        }
        #endregion

        #region GetCartItemsQuantity Method Tests
        [Fact]
        public async void GetCartItemsQuantity_InvalidUserId_ReturnsZero()
        {
            //Arrange
            _cartRepositoryMock.Setup(item => item.GetLoggedUserCartIdAsync(It.IsAny<Guid>()))
                   .ReturnsAsync((int?)null);

            //Act
            int actual = await _cartService.GetCartItemsQuantity();

            //Assert
            actual.Should().Be(0);
        }

        [Fact]
        public async Task GetCartItemsQuantity_ValidCartId_ReturnsQuantity()
        {
            //Arrange
            int expected = 12;
            int cartId = _fixture.Create<int>();
            Guid userId = _fixture.Create<Guid>();

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);
            _cartRepositoryMock.Setup(item => item.GetLoggedUserCartIdAsync(userId))
                               .ReturnsAsync(cartId);
            _cartRepositoryMock.Setup(item => item.GetCartItemsQuantityAsync(cartId))
                               .ReturnsAsync(expected);

            // Act
            int actual = await _cartService.GetCartItemsQuantity();

            // Assert
            actual.Should().Be(expected);
        }
        #endregion

        #region DeleteFromCart Method Tests
        [Fact]
        public async Task DeleteFromCart_InvalidCartId_ReturnsFailureResult()
        {
            //Arrange
            int cartItemId = _fixture.Create<int>();

            _cartRepositoryMock.Setup(item => item.GetCartItemByIdAsync(cartItemId))
                .ReturnsAsync((CartItem?)null);

            //Act
            Result result = await _cartService.DeleteFromCart(cartItemId);

            //Assert
            result.Should().NotBeNull();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartItemErrors.CartItemDoesNotExists);
        }

        [Fact]
        public async Task DeleteFromCart_ValidCartId_ReturnsSuccessResult()
        {
            //Arrange
            var cartItem = _fixture.Build<CartItem>()
                .With(item => item.IsActive, true)
                .Without(item => item.DateDeleted)
                .Create();


            _cartRepositoryMock.Setup(repo => repo.GetCartItemByIdAsync(cartItem.Id))
                .ReturnsAsync(cartItem);

            _cartRepositoryMock.Setup(repo => repo.GetCartByIdAsync(cartItem.CartId))
                .ReturnsAsync(_fixture.Build<Cart>().With(c => c.Id, cartItem.CartId).Create());

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _cartService.DeleteFromCart(cartItem.Id);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            cartItem.IsActive.Should().BeFalse();
            cartItem.DateDeleted.Should().NotBeNull();
        }
        #endregion 

        #region CalculateItemsTotal Method Tests
        [Fact]
        public void CalculateItemsTotal_WithValidItems_ReturnsCorrectTotal()
        {
            //Arrange
            IEnumerable<CartItem> cartItems = new List<CartItem>()
            {

                new CartItem()
                {
                    Quantity = 15,
                    Offer = new Offer { Price = 200 }
                },
                new CartItem()
                {
                    Quantity = 1,
                    Offer = new Offer { Price = 1000 }
                },
                new CartItem()
                {
                    Quantity = 20,
                    Offer = new Offer { Price = 10 }
                }
            };


            decimal expected = 4200;


            //Act
            decimal actual = _cartService.CalculateItemsTotal(cartItems);

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void CalculateItemsTotal_NullList_ReturnsZero()
        {
            //Arrange
            IEnumerable<CartItem>? cartItems = null;

            //Act
            decimal expected = 0;
            decimal actual = _cartService.CalculateItemsTotal(cartItems);

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void CalculateItemsTotal_EmptyList_ReturnsZero()
        {
            //Arrange
            IEnumerable<CartItem> cartItems = new List<CartItem>();

            //Act
            decimal expected = 0;
            decimal actual = _cartService.CalculateItemsTotal(cartItems);

            //Assert
            actual.Should().Be(expected);
        }
        #endregion

        #region CalculateMinimalDeliveryCost Method Tests
        [Fact]
        public void CalculateMinimalDeliveryCost_ItemsWithDeliveryTypes_ReturnsSumOfMinimalPrices()
        {
            //Arrange
            DeliveryType deliveryType1 = _fixture.Build<DeliveryType>().With(item => item.Price, 12).Create();
            DeliveryType deliveryType2 = _fixture.Build<DeliveryType>().With(item => item.Price, 11).Create();
            DeliveryType deliveryType3 = _fixture.Build<DeliveryType>().With(item => item.Price, 69).Create();

            Offer offer1 = _fixture.Build<Offer>().With(item => item.OfferDeliveryTypes, new List<OfferDeliveryType>()
            {
                 new OfferDeliveryType(){ DeliveryType = deliveryType1 },
                 new OfferDeliveryType(){ DeliveryType = deliveryType2 },
            })
            .Create();

            Offer offer2 = _fixture.Build<Offer>().With(item => item.OfferDeliveryTypes, new List<OfferDeliveryType>()
            {
                 new OfferDeliveryType(){ DeliveryType = deliveryType3 },
            })
            .Create();

            IEnumerable<CartItem> cartItems = new List<CartItem>()
            {
                new CartItem()
                {
                    Quantity = 15,
                    Offer = offer1,
                },
                new CartItem()
                {
                    Quantity = 1,
                    Offer = offer2,
                },
            };

            decimal expected = 11 + 69;

            //Act
            decimal actual = _cartService.CalculateMinimalDeliveryCost(cartItems);

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void CalculateMinimalDeliveryCost_EmptyList_ReturnsZero()
        {
            // Arrange
            IEnumerable<CartItem> cartItems = new List<CartItem>();

            // Act
            decimal result = _cartService.CalculateMinimalDeliveryCost(cartItems);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CalculateMinimalDeliveryCost_NullList_ReturnsZero()
        {
            // Arrange
            IEnumerable<CartItem>? cartItems = null;

            // Act
            decimal result = _cartService.CalculateMinimalDeliveryCost(cartItems);

            // Assert
            result.Should().Be(0);
        }
        #endregion
    }
}
