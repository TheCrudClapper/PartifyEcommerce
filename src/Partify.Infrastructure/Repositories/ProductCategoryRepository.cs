using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly DatabaseContext _dbContext;
        public ProductCategoryRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }
        public async Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync()
        {
            return await _dbContext.ProductCategories
                .Where(item => item.IsActive)
                .ToListAsync();
        }
    }
}
