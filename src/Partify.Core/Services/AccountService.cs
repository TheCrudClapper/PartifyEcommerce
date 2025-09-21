using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.AccountDto;
using CSOS.Core.Mappings.ToDomainEntity.ApplicationUserMappings;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
namespace CSOS.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IAccountRepository _accountRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressService _addressService;

        public AccountService
            (UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICurrentUserService currentUserService,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IAddressService addressService,
            RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _accountRepo = accountRepository;
            _signInManager = signInManager;
            _addressService = addressService;
            _currentUserService = currentUserService;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Register(RegisterRequest? request)
        {
            if (request == null)
                return IdentityResult.Failed();

            Cart cart = new Cart() { IsActive = true, DateCreated = DateTime.UtcNow };

            ApplicationUser user = request.ToApplicationUserEntity(cart);
            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
                return createResult;

            if(!await _roleManager.RoleExistsAsync(UserRoleOption.User.ToRoleName()))
            {
                var roleResult = await CreateRole(UserRoleOption.User);
                if (!roleResult.Succeeded)
                    return roleResult;
            }

            await _userManager.AddToRoleAsync(user, UserRoleOption.User.ToRoleName());

            await _signInManager.SignInAsync(user, isPersistent: false);
            return IdentityResult.Success;
        }

        public async Task<SignInResult> Login(LoginRequest? request)
        {
            if (request == null)
                return SignInResult.Failed;

            var result = await _accountRepo.IsUserByEmailInDatabaseAsync(request.Email);

            if (!result)
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: request.IsPersistent, lockoutOnFailure: false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Result<AccountResponse>> GetAccount()
        {
            var userResult = await _currentUserService.GetCurrentUserAsync();

            if (userResult.IsFailure)
                return Result.Failure<AccountResponse>(userResult.Error);

            return userResult.Value.ToAccountResponse();
        }

        public async Task<Result> Edit(AccountUpdateRequest? request)
        {
            if (request == null)
                return Result.Failure(AccountErrors.MissingAccountUpdateRequest);

            var userResult = await _currentUserService.GetCurrentUserAsync();
            if (userResult.IsFailure)
                return Result.Failure(userResult.Error);

            var user = userResult.Value;

            user.Title = request.Title;
            user.PhoneNumber = request.PhoneNumber;
            user.NIP = request.NIP;
            user.FirstName = request.FirstName;
            user.Surname = request.Surname;
            user.DateEdited = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> ChangePassword(PasswordChangeRequest? request)
        {
            if (request == null)
                return Result.Failure(AccountErrors.MissingPasswordChangeRequest);

            var userResult = await _currentUserService.GetCurrentUserAsync();
            if (userResult.IsFailure)
                return Result.Failure(AccountErrors.AccountNotFound);

            var user = userResult.Value;

            if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
                return Result.Failure(AccountErrors.WrongPassword);

            IdentityResult result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
                return Result.Failure(AccountErrors.PasswordChangeFailed);

            return Result.Success();
        }

        public async Task<Result<AccountDetailsResponse>> GetAccountDetailsAsync()
        {
            var addressResult = await _addressService.GetUserAddressForEdit();

            if (addressResult.IsFailure)
                return Result.Failure<AccountDetailsResponse>(AddressErrors.AddressNotFound);

            var accountResult = await GetAccount();
            if (accountResult.IsFailure)
                return Result.Failure<AccountDetailsResponse>(AccountErrors.AccountNotFound);

            var addressResponse = addressResult.Value;

            var accountResponse = accountResult.Value;

            return new AccountDetailsResponse()
            {
                AddressResponse = addressResponse,
                AccountResponse = accountResponse,
            };
        }

        public async Task<bool> DoesCurrentUserHaveAddress()
        {
            Guid userId = _currentUserService.GetUserId();
            var result = await _accountRepo.GetUserWithAddressAsync(userId);

            if (result == null || result.Address == null)
                return false;

            return true;

        }

        private bool IsAllowedRole(UserRoleOption role) =>
                role == UserRoleOption.User || role == UserRoleOption.Admin;

        private async Task<IdentityResult> CreateRole(UserRoleOption roleOption)
        {
            if (!IsAllowedRole(roleOption))
                return IdentityResult.Failed(new IdentityError { Description = "Given Role is not valid one" });

            return await _roleManager.CreateAsync(new ApplicationRole { Name = roleOption.ToRoleName() });
        }

        public Task<bool> IsEmailAlreadyTaken(string email)
        {
            return _accountRepo.IsEmailTakenAsync(email);
        }
    }
}
