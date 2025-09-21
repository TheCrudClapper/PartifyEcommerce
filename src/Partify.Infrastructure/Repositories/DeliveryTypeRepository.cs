using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.Domain.Entities;
using CSOS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class DeliveryTypeRepository : IDeliveryTypeRepository
    {
        private readonly DatabaseContext _dbContext;

        public DeliveryTypeRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DeliveryType>> GetAllDeliveryTypesAsync()
        {
            return await _dbContext.DeliveryTypes
                .Where(dt => dt.IsActive)
                .ToListAsync();
        }
    }
}