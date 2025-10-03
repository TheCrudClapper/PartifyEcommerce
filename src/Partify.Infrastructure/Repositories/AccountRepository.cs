using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly DatabaseContext _dbContext;
    public AccountRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    public async Task AddAsync(ApplicationUser entity, CancellationToken cancellationToken)
    {
        await _dbContext.Users
            .AddAsync(entity, cancellationToken);
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(item => item.IsActive && item.Id == id, cancellationToken);
    }

    public async Task<ApplicationUser?> GetUserWithAddressAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(item => item.Address)
            .FirstOrDefaultAsync(item => item.IsActive && item.Id == userId, cancellationToken);
    }

    public async Task<bool> IsUserByEmailInDatabaseAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
             .AnyAsync(item => item.UserName == email && item.IsActive, cancellationToken);
    }

    public async Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AnyAsync(item => item.Email == email, cancellationToken);
    }

}
