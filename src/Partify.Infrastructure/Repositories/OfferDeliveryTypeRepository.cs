using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Infrastructure.DbContext;

namespace CSOS.Infrastructure.Repositories;

public class OfferDeliveryTypeRepository : IOfferDeliveryTypeRepository
{
    private readonly DatabaseContext _dbContext;
    public OfferDeliveryTypeRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(OfferDeliveryType entity, CancellationToken cancellationToken)
    {
        await _dbContext.OfferDeliveryTypes
            .AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<OfferDeliveryType> entities, CancellationToken cancellationToken)
    {
        await _dbContext.OfferDeliveryTypes
            .AddRangeAsync(entities, cancellationToken);
    }

}
