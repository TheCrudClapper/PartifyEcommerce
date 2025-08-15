using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Helpers;
using CSOS.UI.Mappings.ToViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly IConfigurationReader _configurationReader;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, IPictureHandlerService pictureHandlerService , ILogger<CartController> logger)
        {
            _cartService = cartService;
            _pictureHandlerService = pictureHandlerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            _logger.LogInformation("CartController - GET Index Method called.");
            var result = await _cartService.GetLoggedUserCart();

            if (result.IsFailure)
            {
                _logger.LogError("Error while fetching {UserName} cart. Error: {Error}", User.Identity!.Name, result.Error.Description);
                return View("Error", result.Error.Description);
            }

            _logger.LogInformation("Cart fetched for {UserName}. Total items: {ItemCount}", User.Identity!.Name, result.Value.CartItems.Count());
            var cart = result.Value.ToCartViewModel(_pictureHandlerService);
            return View(cart);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            _logger.LogInformation("CartController - POST AddToCart Method called with offerId: {OfferId} and quantity: {Quantity}",
                id, quantity);

            var result = await _cartService.AddToCart(id, quantity);

            if (result.IsFailure)
            {
                _logger.LogError("Error while adding offer to cart. Error: {Error}", result.Error.Description);
                return Json(new JsonResponseModel { Success = false, Message = $"Error: {result.Error.Description}" });
            }

            _logger.LogInformation("Successfully added offer of Id {OfferId} and quantity {Quantity} to {UserName}'s cart.",
                id, quantity, User.Identity!.Name);
            return Json(new JsonResponseModel { Success = true, Message = "Item Successfully Added to Cart" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            _logger.LogInformation("CartController - POST DeleteFromCart Method called with offerId: {OfferId}", id);
            var result = await _cartService.DeleteFromCart(id);

            if (result.IsFailure)
            {
                _logger.LogError("Error while deleting offer from cart, Error: {Error}", result.Error.Description);
                return Json(new JsonResponseModel { Success = false, Message = $"Error: {result.Error.Description}" });
            }

            _logger.LogInformation("Successfully deleted offer of Id {OfferId} from {UserName}'s cart", id, User.Identity!.Name);
            return Json(new JsonResponseModel { Success = true, Message = "Item removed from cart successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantityInCart(int cartItemId, int quantity)
        {
            _logger.LogInformation("CartController - POST UpdateQuantityInCart Method called with cartItemId: {CartItemId} and quantity: {Quantity}",
                cartItemId, quantity);

            var result = await _cartService.UpdateCartItemQuantity(cartItemId, quantity);

            if (result.IsFailure)
            {
                _logger.LogError("Error while updating quantity of item with Id: {CartItemId}. Error: {Error}",
                    cartItemId, result.Error.Description);
                return Json(new JsonResponseModel { Success = false, Message = $"Error: {result.Error.Description}" });
            }

            _logger.LogInformation("Successfully updated quantity for cartItem with Id {Id} for User: {UserName}",
                cartItemId, User.Identity!.Name);
            return Json(new JsonResponseModel { Success = true, Message = "Updated cart successfully!" });
        }

        [HttpGet]
        public IActionResult GetCartItemsCount()
        {
            return ViewComponent("NavbarCart");
        }
    }
}