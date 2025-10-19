using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.Core.DTO.OfferDto;
using CSOS.Core.Helpers;
using CSOS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories;

public class OfferRepository : IOfferRepository
{
    private readonly DatabaseContext _dbContext;
    public OfferRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAsync(Offer entity, CancellationToken cancellationToken)
    {
        await _dbContext.Offers
            .AddAsync(entity, cancellationToken);
    }
    public async Task<Offer?> GetOfferWithDetailsToEditAsync(int id, Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Offers
          .Where(item => item.Id == id && item.IsActive && item.SellerId == userId)
          .Include(item => item.Product)
              .ThenInclude(item => item.ProductImages)
          .Include(item => item.OfferDeliveryTypes)
              .ThenInclude(item => item.DeliveryType)
          .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Offer?> GetOfferByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Offers
            .FirstOrDefaultAsync(item => item.Id == id && item.IsActive, cancellationToken);
    }

    public async Task<Offer?> GetUserOfferByIdAsync(int offerId, Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Offers
            .FirstOrDefaultAsync(item => item.Id == offerId && item.SellerId == userId && item.IsActive == true, cancellationToken);
    }
    public async Task<Offer?> GetOfferWithAllDetailsByUserAsync(int offerId, Guid userId, CancellationToken cancellationToken)
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
        .FirstOrDefaultAsync(cancellationToken);

    }
    public async Task<bool> IsOfferInDatabaseAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Offers
            .AnyAsync(item => item.Id == id && item.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<UserOfferResponse>> GetFilteredUserOffersAsync(string? title, Guid userId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Offers
            .AsNoTracking()
            .Where(item => item.IsActive && item.SellerId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            string phrase = title.Trim();
            query = query.Where(item => item.Product.ProductName.Contains(phrase));
        }

        return await query.Select(o => new UserOfferResponse
        {
            Id = o.Id,
            ProductName = o.Product.ProductName,
            Price = o.Price,
            ImageUrl = o.Product.ProductImages
                    .Where(img => img.IsActive)
                    .Select(img => img.ImagePath)
                    .FirstOrDefault() ?? string.Empty,
            IsOfferPrivate = o.IsOfferPrivate,
            ProductCategory = o.Product.ProductCategory.Name,
            ProductCondition = o.Product.Condition.ConditionTitle,
            DateCreated = o.DateCreated,
            StockQuantity = o.StockQuantity,
        })
        .ToListAsync(cancellationToken);
    }
    public async Task<OfferResponse?> GetOfferWithAllDetailsAsync(int id, Guid? userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Offers
        .AsNoTracking()
        .Where(o => o.IsActive && !o.IsOfferPrivate && o.Id == id)
        .Select(o => new OfferResponse
        {
            Id = o.Id,
            ProductCondition = o.Product.Condition.ConditionTitle,
            DateCreated = o.DateCreated.Date,
            IsLiked = userId != null
                    ? o.LikedOffers
                        .Any(lo => lo.UserId == userId && lo.OfferId == o.Id && lo.IsActive)
                    : false,
            Seller = o.Seller.UserName!,
            Description = o.Product.Description,
            ProductCategory = o.Product.ProductCategory.Name,
            Price = o.Price,
            StockQuantity = o.StockQuantity,
            IsSellerCompany = o.Seller.NIP != null,
            Title = o.Product.ProductName,
            Place = o.Seller.Address!.Place ?? string.Empty,
            PostalCity = o.Seller.Address.PostalCity ?? string.Empty,
            PostalCode = o.Seller.Address.PostalCode ?? string.Empty,
            ProductImages = o.Product.ProductImages
                        .Where(item => item.IsActive)
                        .Select(item => item.ImagePath)
                        .ToList(),
            AvaliableDeliveryTypes = o.OfferDeliveryTypes
                    .Select(item => new DeliveryTypeResponse()
                    {
                        Title = item.DeliveryType.Title,
                        Price = item.DeliveryType.Price,
                        Id = item.DeliveryType.Id
                    }).ToList()
        })
        .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<IEnumerable<OfferIndexResponse>> GetFilteredOffersAsync(OfferFilter filter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Offers
        .AsNoTracking()
        .Where(o => o.IsActive && !o.IsOfferPrivate)
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
        {
            var phrase = filter.SearchPhrase.Trim();
            query = query.Where(o =>
                o.Product.ProductName.Contains(phrase) ||
                o.Product.Description.Contains(phrase));
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

        return await query
            .Select(offer => new OfferIndexResponse
            {
                Id = offer.Id,
                ProductName = offer.Product.ProductName,
                ProductCategory = offer.Product.ProductCategory.Name,
                ProductCondition = offer.Product.Condition.ConditionTitle,
                DateCreated = offer.DateCreated,
                Price = offer.Price,
                Seller = offer.Seller.UserName!,
                Description = offer.Product.Description,
                StockQuantity = offer.StockQuantity,
                ImageUrl = offer.Product.ProductImages
                    .Where(img => img.IsActive)
                    .Select(path => path.ImagePath)
                    .FirstOrDefault() ?? string.Empty
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetNonPrivateOfferCount(CancellationToken cancellationToken)
    {
        return await _dbContext.Offers
            .CountAsync(item => item.IsActive && !item.IsOfferPrivate, cancellationToken);
    }

    public async Task<IEnumerable<Offer>> GetOffersByTakeAsync(int take = 12, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Offers
           .Where(item => item.IsActive && !item.IsOfferPrivate)
           .Include(item => item.Product)
           .ThenInclude(item => item.ProductImages)
           .OrderBy(item => item.DateCreated)
           .Take(take)
           .ToListAsync(cancellationToken);
    }
}