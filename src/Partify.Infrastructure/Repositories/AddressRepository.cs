using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using CSOS.Core.Domain.Entities;
using CSOS.Infrastructure.DbContext;

namespace CSOS.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly DatabaseContext _dbContext;
    public AddressRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    public async Task AddAsync(Address address, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(address, cancellationToken);
    }

    public async Task<Address?> GetAddressByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Addresses
            .Include(item => item.Country)
            .FirstOrDefaultAsync(item => item.Id == id && item.IsActive, cancellationToken);
    }

    public async Task<ApplicationUser?> GetUserWithAddressAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Include(item => item.Address)
            .FirstOrDefaultAsync(item => item.IsActive && item.Id == userId, cancellationToken);
    }
}
