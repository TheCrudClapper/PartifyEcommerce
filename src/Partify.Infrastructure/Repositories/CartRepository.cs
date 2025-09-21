using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DatabaseContext _dbContext;
        public CartRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CartItem cartItem)
        {
            await _dbContext.CartItems.AddAsync(cartItem);
        }

        public async Task<IEnumerable<CartItem>?> GetCartItemsForCostsUpdateAsync(int cartId)
        {
            return await _dbContext.CartItems
                .Where(cartItem => cartItem.CartId == cartId && cartItem.IsActive)
                .Include(offer => offer.Offer)
                    .ThenInclude(item => item.OfferDeliveryTypes)
                        .ThenInclude(item => item.DeliveryType)
                .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int offerId)
        {
            return await _dbContext.CartItems
                .FirstOrDefaultAsync(item => item.CartId == cartId && item.OfferId == offerId && item.IsActive);
        }
        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            return await _dbContext.CartItems
                .FirstOrDefaultAsync(item => item.Id == cartItemId && item.IsActive);
        }

        public async Task<int> GetCartItemsQuantityAsync(int cartId)
        {
            return await _dbContext.CartItems.Where(item => item.CartId == cartId && item.IsActive)
                .SumAsync(item => item.Quantity);
        }

        public async Task<CartItem?> GetCartItemWithOfferAsync(int cartItemId)
        {
            return await _dbContext.CartItems
                 .Include(item => item.Offer)
                 .FirstOrDefaultAsync(item => item.Id == cartItemId && item.IsActive);
        }

        public async Task<Cart?> GetCartWithAllDetailsAsync(int cartId)
        {
            return await _dbContext.Carts
                .Include(cart => cart.CartItems)
                    .ThenInclude(cartItem => cartItem.Offer)
                        .ThenInclude(offer => offer.Product)
                            .ThenInclude(product => product.ProductCategory)
                .Include(cart => cart.CartItems)
                    .ThenInclude(cartItem => cartItem.Offer)
                        .ThenInclude(offer => offer.Product)
                            .ThenInclude(product => product.Condition)
                .Include(cart => cart.CartItems)
                    .ThenInclude(cartItem => cartItem.Offer)
                        .ThenInclude(offer => offer.Product)
                            .ThenInclude(product => product.ProductImages)
                .FirstOrDefaultAsync(cart => cart.Id == cartId && cart.IsActive);
        }

        public async Task<int?> GetLoggedUserCartIdAsync(Guid userId)
        {
            return await _dbContext.Users
               .Where(user => user.IsActive && user.Id == userId && user.Cart.IsActive)
               .Select(item => item.Cart.Id)
               .FirstOrDefaultAsync();
        }

        public async Task<Cart?> GetCartByIdAsync(int cartId)
        {
            return await _dbContext.Carts
                .FirstOrDefaultAsync(item => item.Id == cartId && item.IsActive);
        }

      
    }
}
