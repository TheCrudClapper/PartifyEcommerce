using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Infrastructure.DbContext;

namespace CSOS.Infrastructure.Repositories
{
    public class OfferDeliveryTypeRepository : IOfferDeliveryTypeRepository
    {
        private readonly DatabaseContext _dbContext;
        public OfferDeliveryTypeRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(OfferDeliveryType entity)
        {
            await _dbContext.OfferDeliveryTypes.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<OfferDeliveryType> entities)
        {
            await _dbContext.OfferDeliveryTypes.AddRangeAsync(entities);
        }

    }
}
