using CSOS.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using CSOS.Core.Domain.Entities;
using CSOS.Infrastructure.DbContext;

namespace CSOS.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DatabaseContext _dbContext;

        public CountryRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _dbContext.Countries
                .Where(c => c.IsActive)
                .ToListAsync();
        }
    }
}