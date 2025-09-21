using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.CartDto;
using CSOS.Core.ResultTypes;

namespace CSOS.Core.ServiceContracts
{
    public interface ICartService
    {
        /// <summary>
        /// AddAsync to cart an offer of given id and given quantity
        /// </summary>
        /// <param name="offerId">Offer Id that will be added to cart</param>
        /// <param name="quantity">Quantity of Offer</param>
        /// <returns>Return Result object identicating Failure or Success</returns>
        Task<Result> AddToCart(int offerId, int quantity = 1);

        /// <summary>
        /// Delete from cart cart item of given ID
        /// </summary>
        /// <param name="cartItemId">Cart item Id  to be deleted from cart</param>
        /// <returns>Return Result object identicating Failure or Success</returns>
        Task<Result> DeleteFromCart(int cartItemId);

        /// <summary>
        /// Returns curently logged user cart
        /// </summary>
        /// <returns>Return Result object identicating Failure or Success with CartResponseDto Object</returns>
        Task<Result<CartResponseDto>> GetLoggedUserCart();

        /// <summary>
        /// Gets current logged user cart id
        /// </summary>
        /// <returns>Returns an int representing user cart id</returns>
        Task<Result<int>> GetLoggedUserCartId();

        /// <summary>
        /// Updated total cart value after deleting, updating, adding
        /// </summary>
        /// <param name="cartId">Id of cart</param>
        /// <returns></returns>
        Task<Result> UpdateTotalCartValue(int cartId);

        /// <summary>
        /// Updates quantity of already existing cart item in cart
        /// </summary>
        /// <param name="cartItemId">Id of cart item</param>
        /// <param name="quantity">Quantity to be added to cart</param>
        /// <returns>Return Result object identicating Failure or Success</returns>
        Task<Result> UpdateCartItemQuantity(int cartItemId, int quantity);

        /// <summary>
        /// Counts user items in cart
        /// </summary>
        /// <returns>Returns integer representing amount of items in cart</returns>
        Task<int> GetCartItemsQuantity();

        /// <summary>
        /// Helper method, calculates total items costs
        /// </summary>
        /// <param name="cartItems">IEnumerable collection representing items in cart</param>
        /// <returns>Return an decimal value with total cost</returns>
        decimal CalculateItemsTotal(IEnumerable<CartItem>? cartItems);

        /// <summary>
        /// Calculates and sums lowest deliveries avaliable for each item in cart and sums them
        /// </summary>
        /// <param name="cartItems"></param>
        /// <returns>Returns an decimal value with total sum of minimal deliveries costs</returns>
        decimal CalculateMinimalDeliveryCost(IEnumerable<CartItem>? cartItems);
    }
}
