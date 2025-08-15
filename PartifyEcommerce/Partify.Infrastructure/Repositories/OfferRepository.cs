using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.Helpers;
using CSOS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly DatabaseContext _dbContext;
        public OfferRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Offer entity)
        {
            await _dbContext.Offers.AddAsync(entity);
        }
        public async Task<Offer?> GetOfferWithDetailsToEditAsync(int id, Guid userId)
        {
            return await _dbContext.Offers
              .Where(item => item.Id == id && item.IsActive && item.SellerId == userId)
              .Include(item => item.Product)
                  .ThenInclude(item => item.ProductImages)
              .Include(item => item.OfferDeliveryTypes)
                  .ThenInclude(item => item.DeliveryType)
              .FirstOrDefaultAsync();
        }
        public async Task<Offer?> GetOfferByIdAsync(int id)
        {
            return await _dbContext.Offers
                .FirstOrDefaultAsync(item => item.Id == id && item.IsActive);
        }

        public async Task<Offer?> GetUserOffersByIdAsync(int offerId, Guid userId)
        {
            return await _dbContext.Offers
                .FirstOrDefaultAsync(item => item.Id == offerId && item.SellerId == userId && item.IsActive == true);
        }
        public async Task<Offer?> GetOfferWithAllDetailsByUserAsync(int offerId, Guid userId)
        {
            return await _dbContext.Offers
            .AsNoTracking()
            .Where(item => item.IsActive && item.SellerId == userId && item.Id == offerId)
            .Include(item => item.Product)
                .ThenInclude(item => item.ProductImages)
            .Include(item => item.Product.ProductCategory)
            .Include(item => item.Product.Condition)
            .Include(item => item.OfferDeliveryTypes)
                .ThenInclude(item => item.DeliveryType)
            .FirstOrDefaultAsync();

        }
        public async Task<bool> IsOfferInDbAsync(int id)
        {
            return await _dbContext.Offers
                .AnyAsync(item => item.Id == id && item.IsActive);
        }

        public async Task<IEnumerable<Offer>> GetFilteredUserOffersAsync(string? title, Guid userId)
        {
            var query = _dbContext.Offers.Where(item => item.IsActive)
              .Where(item => item.SellerId == userId)
              .Include(item => item.Product)
              .ThenInclude(item => item.ProductImages)
              .Include(item => item.Product.Condition)
              .Include(item => item.Product.ProductCategory)
              .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(item => item.Product.ProductName.Contains(title));

            return await query.ToListAsync();
        }
        public async Task<Offer?> GetOfferWithAllDetailsAsync(int id)
        {
            return await _dbContext.Offers
            .Where(o => o.IsActive && !o.IsOfferPrivate && o.Id == id)
            .Include(o => o.Seller)
                .ThenInclude(o => o.Address)
            .Include(o => o.Product)
                .ThenInclude(p => p.ProductImages)
            .Include(o => o.Product.ProductCategory)
            .Include(o => o.Product.Condition)
            .Include(o => o.OfferDeliveryTypes)
                .ThenInclude(o => o.DeliveryType)
            .Include(o => o.LikedOffers)
            .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Offer>> GetFilteredOffersAsync(OfferFilter filter)
        {
            var query = _dbContext.Offers
            .Where(o => o.IsActive && !o.IsOfferPrivate)
            .Include(o => o.Seller)
            .Include(o => o.Product)
                .ThenInclude(p => p.ProductImages)
            .Include(o => o.Product.ProductCategory)
            .Include(o => o.Product.Condition)
            .Include(o => o.OfferDeliveryTypes)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                query = query.Where(o =>
                    o.Product.ProductName.Contains(filter.SearchPhrase) ||
                    o.Product.Description.Contains(filter.SearchPhrase));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(o => o.Product.ProductCategoryId == filter.CategoryId);
            }

            if (filter.PriceFrom.HasValue)
            {
                query = query.Where(o => o.Price >= filter.PriceFrom.Value);
            }

            if (filter.PriceTo.HasValue)
            {
                query = query.Where(o => o.Price <= filter.PriceTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.DeliveryOption) &&
                int.TryParse(filter.DeliveryOption, out int deliveryId))
            {
                query = query.Where(o =>
                    o.OfferDeliveryTypes.Any(dt => dt.DeliveryTypeId == deliveryId));
            }

            if (filter.SortOption == "price_asc")
            {
                query = query.OrderBy(o => o.Price);
            }
            else if (filter.SortOption == "price_desc")
            {
                query = query.OrderByDescending(o => o.Price);
            }

            return await query.ToListAsync();
        }
        public async Task<int> GetNonPrivateOfferCount()
        {
            return await _dbContext.Offers
                .CountAsync(item => item.IsActive && !item.IsOfferPrivate);
        }

        public async Task<IEnumerable<Offer>> GetOffersByTakeAsync(int take = 12)
        {
            return await _dbContext.Offers
               .Where(item => item.IsActive && !item.IsOfferPrivate)
               .Include(item => item.Product)
               .ThenInclude(item => item.ProductImages)
               .OrderBy(item => item.DateCreated)
               .Take(take)
               .ToListAsync();
        }
    }
}