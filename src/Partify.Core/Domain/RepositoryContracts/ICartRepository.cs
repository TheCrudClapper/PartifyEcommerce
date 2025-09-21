using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for managing shopping cart and cart items.
    /// </summary>
    public interface ICartRepository
    {
        /// <summary>
        /// Asynchronously adds a new <see cref="CartItem"/> to the database.
        /// </summary>
        /// <param name="cartItem">The cart item to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(CartItem cartItem);

        /// <summary>
        /// Asynchronously retrieves a cart item by cart ID and offer ID.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="offerId">The ID of the offer.</param>
        /// <returns>The matching <see cref="CartItem"/> if found; otherwise, <c>null</c>.</returns>
        Task<CartItem?> GetCartItemAsync(int cartId, int offerId);

        /// <summary>
        /// Asynchronously retrieves a cart item by its ID.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item.</param>
        /// <returns>The matching <see cref="CartItem"/> if found; otherwise, <c>null</c>.</returns>
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);

        /// <summary>
        /// Asynchronously retrieves a cart by its ID.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <returns>The matching <see cref="Cart"/> if found; otherwise, <c>null</c>.</returns>
        Task<Cart?> GetCartByIdAsync(int cartId);

        /// <summary>
        /// Asynchronously gets the total quantity of items in the specified cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <returns>The total quantity of cart items.</returns>
        Task<int> GetCartItemsQuantityAsync(int cartId);

        /// <summary>
        /// Asynchronously retrieves the cart ID for the user with the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The cart ID if found; otherwise, <c>null</c>.</returns>
        Task<int?> GetLoggedUserCartIdAsync(Guid userId);

        /// <summary>
        /// Asynchronously retrieves a cart item with its associated offer eagerly loaded.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item.</param>
        /// <returns>The matching <see cref="CartItem"/> with offer details if found; otherwise, <c>null</c>.</returns>
        Task<CartItem?> GetCartItemWithOfferAsync(int cartItemId);

        /// <summary>
        /// Asynchronously retrieves a cart with all related navigation properties eagerly loaded.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <returns>The matching <see cref="Cart"/> with all details if found; otherwise, <c>null</c>.</returns>
        Task<Cart?> GetCartWithAllDetailsAsync(int cartId);

        /// <summary>
        /// Asynchronously retrieves all cart items for a given cart ID, including necessary details for cost updates.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <returns>A collection of <see cref="CartItem"/> entities; or <c>null</c> if no items are found.</returns>
        Task<IEnumerable<CartItem>?> GetCartItemsForCostsUpdateAsync(int cartId);
    }
}
