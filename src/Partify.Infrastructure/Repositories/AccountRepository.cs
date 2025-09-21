using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DatabaseContext _dbContext;
        public AccountRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task AddAsync(ApplicationUser entity)
        {
            await _dbContext.Users.AddAsync(entity);
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(Guid id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(item => item.IsActive && item.Id == id);
        }

        public async Task<ApplicationUser?> GetUserWithAddressAsync(Guid userId)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Where(item => item.IsActive && item.Id == userId)
                .Include(item => item.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserByEmailInDatabaseAsync(string email)
        {
            return await _dbContext.Users
                 .AnyAsync(item => item.UserName == email && item.IsActive);
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(item => item.Email == email);
        }

    }
}
