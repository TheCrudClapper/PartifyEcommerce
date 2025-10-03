using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.Address;
using CSOS.Core.Mappings.ToDomainEntity.AddressMappings;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services;

public class AddressService : IAddressService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IAccountRepository _accountRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddressService(ICurrentUserService currentUserService,
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork,
        IAccountRepository accountRepository)
    {
        _currentUserService = currentUserService;
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
    }

    public async Task<Result> AddAddress(AddressAddRequest? request, CancellationToken cancellationToken)
    {
        if (request == null)
            return Result.Failure(AddressErrors.AddressAddRequestIsNull);

        var currentUserId = _currentUserService.GetUserId();

        Address address = request.ToAddressEntity(currentUserId);

        await _addressRepository.AddAsync(address, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> EditUserAddress(AddressUpdateRequest? request, CancellationToken cancellationToken)
    {
        if (request == null)
            return Result.Failure(AddressErrors.MissingAddressUpdateRequest);

        var address = await _addressRepository.GetAddressByIdAsync(request.Id);

        if (address == null)
            return Result.Failure(AddressErrors.AddressNotFound);

        address.Street = request.Street;
        address.HouseNumber = request.HouseNumber;
        address.CountryId = request.CountryId;
        address.DateEdited = DateTime.UtcNow;
        address.Place = request.Place;
        address.PostalCity = request.PostalCity;
        address.PostalCode = request.PostalCode;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<AddressResponse>> GetUserAddressForEdit(CancellationToken cancellationToken)
    {
        var userAndAddress = await GetCurrentUserWithAddress(cancellationToken);

        if (userAndAddress == null)
            return Result.Failure<AddressResponse>(AddressErrors.AddressNotFound);

        return userAndAddress.ToAddressResponse();
    }

    public async Task<Result<UserAddressDetailsResponse>> GetUserAddressDetails(CancellationToken cancellationToken)
    {
        var userAndAddress = await GetCurrentUserWithAddress(cancellationToken);

        if (userAndAddress == null)
            return Result.Failure<UserAddressDetailsResponse>(AddressErrors.MissingAddressData);

        return userAndAddress.ToUserAddressDetailsResponse();
    }

    private async Task<ApplicationUser?> GetCurrentUserWithAddress(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        return await _accountRepository.GetUserWithAddressAsync(userId, cancellationToken);
    }

    public async Task<Result<AddressResponse>> GetAddress(int addressId, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetAddressByIdAsync(addressId, cancellationToken);
        if (address == null)
            return Result.Failure<AddressResponse>(AddressErrors.AddressNotFound);

        return address.ToAddressResponse();

    }
}
