using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.ServiceContracts;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using AutoFixture;
using CSOS.Core.DTO.Account;
using CSOS.Core.ResultTypes;
using CSOS.Core.Services;

namespace CSOS.Tests
{
    public class AccountServiceTests
    {
        private readonly AccountService _accountService;

        private readonly Mock<IUserStore<ApplicationUser>> _userStoreMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
        private readonly Mock<IAccountRepository> _accountRepoMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IAddressService> _addressServiceMock = new();
        private readonly IFixture _fixture;

        public AccountServiceTests()
        {
            var userManager = new UserManager<ApplicationUser>(
                _userStoreMock.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>());

            var signInManager = new SignInManager<ApplicationUser>(
                userManager,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
                Mock.Of<IAuthenticationSchemeProvider>(),
                Mock.Of<IUserConfirmation<ApplicationUser>>());

            var roleManager = new RoleManager<ApplicationRole>(
                Mock.Of<IRoleStore<ApplicationRole>>(),
                new List<IRoleValidator<ApplicationRole>> { new RoleValidator<ApplicationRole>() },
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<ILogger<RoleManager<ApplicationRole>>>()
            );


            _fixture = new Fixture();

            _accountService = new AccountService(
                userManager,
                signInManager,
                _currentUserServiceMock.Object,
                _accountRepoMock.Object,
                _unitOfWorkMock.Object,
                _addressServiceMock.Object,
                 roleManager);

            //_fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            // .ForEach(b => _fixture.Behaviors.Remove(b));
            //_fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        }

        #region GetAccountForEdit Method Tests
        [Fact]
        public async Task GetAccountForEdit_UserResultFailure_ReturnFailureResult()
        {
            //Arrange
            _currentUserServiceMock.Setup(item => item.GetCurrentUserAsync()).ReturnsAsync(Result.Failure<ApplicationUser>(AccountErrors.AccountNotFound));

            //Act
            var result = await _accountService.GetAccount();

            //Assert
            result.Should().BeOfType<Result<AccountResponse>>();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AccountErrors.AccountNotFound);

        }

        [Fact]
        public async Task GetAccountForEdit_ValidUserId_ReturnsDto()
        {
            //Arrange
            ApplicationUser applicationUser = _fixture.Build<ApplicationUser>()
                .Without(item => item.Address)
                .Without(item => item.Offers)
                .Without(item => item.Cart)
                .Without(item => item.LikedOffers)
                .Create();

            _currentUserServiceMock.Setup(item => item.GetCurrentUserAsync()).ReturnsAsync(applicationUser);

            //Act
            var result = await _accountService.GetAccount();

            //Assert
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<AccountResponse>();
            result.IsSuccess.Should().BeTrue();
            result.Value.PhoneNumber.Should().Be(applicationUser.PhoneNumber);
            result.Value.NIP.Should().Be(applicationUser.NIP);
            result.Value.Surname.Should().Be(applicationUser.Surname);
            result.Value.FirstName.Should().Be(applicationUser.FirstName);
        }
        #endregion

        #region EditUserAddress Method Tests

        [Fact]
        public async Task Edit_UserResultFailure_ReturnFailureResult()
        {
            //Arrange 
            AccountUpdateRequest dto = _fixture.Create<AccountUpdateRequest>();
            _currentUserServiceMock.Setup(item => item.GetCurrentUserAsync()).ReturnsAsync(Result.Failure<ApplicationUser>(AccountErrors.AccountNotFound));

            //Act
            var result = await _accountService.Edit(dto);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AccountErrors.AccountNotFound);
        }

        [Fact]
        public async Task Edit_ValidUser_ReturnsDto()
        {
            //Arrange
            AccountUpdateRequest dto = _fixture.Create<AccountUpdateRequest>();
            ApplicationUser applicationUser = _fixture.Build<ApplicationUser>()
                .Without(item => item.Address)
                .Without(item => item.Offers)
                .Without(item => item.Cart)
                .Without(item => item.LikedOffers)
                .Create();

            _currentUserServiceMock.Setup(item => item.GetCurrentUserAsync()).ReturnsAsync(Result.Success(applicationUser));
            _unitOfWorkMock.Setup(item => item.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

            //Act
            var result = await _accountService.Edit(dto);

            //Assert
            result.IsSuccess.Should().BeTrue();
            applicationUser.PhoneNumber.Should().Be(dto.PhoneNumber);
            applicationUser.Title.Should().Be(dto.Title);
            applicationUser.FirstName.Should().Be(dto.FirstName);
            applicationUser.Surname.Should().Be(dto.Surname);
            applicationUser.NIP.Should().Be(dto.NIP);
            applicationUser.DateEdited.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
        #endregion

    }
}
