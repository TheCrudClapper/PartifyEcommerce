using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.CartDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOfferRepository _offerRepo;
        public CartService(ICurrentUserService currentUserService, IOfferRepository offerRepository, ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            _cartRepo = cartRepository;
            _offerRepo = offerRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> AddToCart(int offerId, int quantity = 1)
        {
            if (quantity <= 0)
                return Result.Failure(CartItemErrors.QuantityLowerThanZero);

            var offer = await _offerRepo.GetOfferByIdAsync(offerId);

            if (offer == null)
                return Result.Failure(OfferErrors.OfferDoesNotExist);

            var cartIdResult = await GetLoggedUserCartId();

            if(cartIdResult.IsFailure)
                return Result.Failure(cartIdResult.Error);

            var existingCartItem = await _cartRepo.GetCartItemAsync(cartIdResult.Value, offer.Id);

            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity + quantity <= offer.StockQuantity)
                {
                    existingCartItem.Quantity += quantity;
                    existingCartItem.DateCreated = DateTime.UtcNow;
                }
                else
                    return Result.Failure(CartItemErrors.CannotAddMoreToCart);
            }
            else
            {
                if (quantity > offer.StockQuantity)
                    return Result.Failure(CartItemErrors.CannotAddMoreToCart);

                CartItem cartItem = new CartItem()
                {
                    CartId = cartIdResult.Value,
                    DateCreated = DateTime.UtcNow,
                    IsActive = true,
                    Offer = offer,
                    Quantity = quantity,
                };

                await _cartRepo.AddAsync(cartItem);
            }

            var result = await SaveAndUpdateCart(cartIdResult.Value);

            if (result.IsFailure)
                return Result.Failure(result.Error);

            return Result.Success();
        }

        public async Task<Result> DeleteFromCart(int cartItemId)
        {
            var cartItem = await _cartRepo.GetCartItemByIdAsync(cartItemId);

            if (cartItem == null)
                return Result.Failure(CartItemErrors.CartItemDoesNotExists);

            var cartId = cartItem.CartId;

            //Soft Delete
            cartItem.IsActive = false;
            cartItem.DateDeleted = DateTime.UtcNow;

            var result = await SaveAndUpdateCart(cartId);

            if (result.IsFailure)
                return Result.Failure(result.Error);

            return Result.Success();
        }

        public async Task<Result<CartResponseDto>> GetLoggedUserCart()
        {
            var cartIdResult = await GetLoggedUserCartId();

            if (cartIdResult.IsFailure)
                return Result.Failure<CartResponseDto>(cartIdResult.Error);

            var cart = await _cartRepo.GetCartWithAllDetailsAsync(cartIdResult.Value);

            if (cart == null)
                return Result.Failure<CartResponseDto>(CartErrors.CartDoesNotExist(cartIdResult.Value));

            var dto = cart.ToCartResponseDto();

            return dto;
        }

        public async Task<Result<int>> GetLoggedUserCartId()
        {
            Guid userId = _currentUserService.GetUserId();

            var cartId = await _cartRepo.GetLoggedUserCartIdAsync(userId);

            if (!cartId.HasValue) 
                return Result.Failure<int>(CartErrors.CartDoesNotExists);

            return cartId.Value;
        }

        public async Task<Result> UpdateTotalCartValue(int cartId)
        {
            IEnumerable<CartItem>? cartItems = await _cartRepo.GetCartItemsForCostsUpdateAsync(cartId);
            
            if(cartItems == null)
                return Result.Failure(CartErrors.CartDoesNotExists);

            //return early, bc nothing to calculate here
            if (cartItems.Count() == 0)
                return Result.Success();

            var totalValue = CalculateItemsTotal(cartItems);

            var minimalDeliveryValue = CalculateMinimalDeliveryCost(cartItems);

            var cart = await _cartRepo.GetCartByIdAsync(cartId);

            if (cart == null)
                return Result.Failure(CartErrors.CartDoesNotExists);

            cart.TotalItemsValue = totalValue;
            cart.TotalCartValue = totalValue + minimalDeliveryValue;
            cart.MinimalDeliveryValue = minimalDeliveryValue;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            var existingItem = await _cartRepo.GetCartItemWithOfferAsync(cartItemId);

            if (existingItem == null)
                return Result.Failure(CartItemErrors.CartItemDoesNotExists);

            if (existingItem.Offer == null)
                return Result.Failure(OfferErrors.OfferIsNull);

            if (quantity <= 0)
            {
                return await DeleteFromCart(cartItemId);
            }

            if (quantity <= existingItem.Offer.StockQuantity)
            {
                existingItem.Quantity = quantity;
                existingItem.DateEdited = DateTime.UtcNow;
            }
            else
            {
                existingItem.Quantity = existingItem.Offer.StockQuantity;
                existingItem.DateEdited = DateTime.UtcNow;
                return Result.Failure(CartItemErrors.CannotAddMoreToCart);
            }

            var result = await  SaveAndUpdateCart(existingItem.CartId);

            if (result.IsFailure)
                return Result.Failure(result.Error);

            return Result.Success();
        }

        public async Task<int> GetCartItemsQuantity()
        {
            var cartIdResult = await GetLoggedUserCartId();

            if (cartIdResult.IsFailure)
                return 0;

            return await _cartRepo.GetCartItemsQuantityAsync(cartIdResult.Value);
        }

        public decimal CalculateItemsTotal(IEnumerable<CartItem>? cartItems)
        {
            if(cartItems == null || cartItems.Count() == 0)
                return 0;

            return cartItems.Sum(item => item.Quantity * item.Offer.Price);
        }

        public decimal CalculateMinimalDeliveryCost(IEnumerable<CartItem>? cartItems)
        {
            if (cartItems == null || cartItems.Count() == 0)
                return 0;

            return cartItems.Select(item => item.Offer.OfferDeliveryTypes
                    .Select(item => item.DeliveryType.Price)
                    .DefaultIfEmpty(0)
                    .Min())
                .Sum();
        }

        private async Task<Result> SaveAndUpdateCart(int cartId)
        {
            await _unitOfWork.SaveChangesAsync();

            var updateResult = await UpdateTotalCartValue(cartId);

            if (updateResult.IsFailure)
                return Result.Failure(updateResult.Error);

            return Result.Success();
        } 
    }
}
