using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories;

public class LikeOfferRepository : ILikeOfferRepository
{
    private readonly DatabaseContext _dbContext;
    public LikeOfferRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    public async Task AddOrReactivateLikeAsync(LikedOffer likedOffer, CancellationToken cancellationToken)
    {
        var existing = await GetLikedOfferAsync(likedOffer.OfferId,
            likedOffer.UserId, cancellationToken);

        if (existing != null)
        {
            existing.IsActive = true;
            existing.DateEdited = DateTime.UtcNow;
        }
        else
        {
            likedOffer.IsActive = true;
            likedOffer.DateCreated = DateTime.UtcNow;
            await _dbContext.LikedOffers.AddAsync(likedOffer, cancellationToken);
        }
    }

    public Task RemoveLikeAsync(LikedOffer likedOffer, CancellationToken cancellationToken)
    {
        likedOffer.IsActive = false;
        likedOffer.DateDeleted = DateTime.UtcNow;
        return Task.CompletedTask;
    }

    public async Task<LikedOffer?> GetLikedOfferAsync(int likedOfferId, Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.LikedOffers
            .Include(item => item.Offer)
            .FirstOrDefaultAsync(item => item.OfferId == likedOfferId && item.UserId == userId, cancellationToken);
    }

    public async Task<IEnumerable<Offer>> GetAllUserLikedOffersAsync(Guid userId, string? title, CancellationToken cancellationToken)
    {
        var query = _dbContext.LikedOffers
            .Where(like => like.UserId == userId &&
                like.IsActive &&
                like.Offer.IsActive &&
                like.Offer.Product.IsActive)
            .Include(like => like.Offer.Product)
            .Select(like => like.Offer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(item => item.Product.ProductName.Contains(title));
        }

        return await query.ToListAsync(cancellationToken);
    }
}
