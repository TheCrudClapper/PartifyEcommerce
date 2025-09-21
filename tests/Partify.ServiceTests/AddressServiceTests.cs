using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.ServiceContracts;
using Moq;
using AutoFixture;
using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.AddressDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.Services;
using FluentAssertions;

namespace CSOS.Tests
{
    public class AddressServiceTests
    {
        private readonly IAddressService _addressService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountRepository _accountRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IAccountRepository> _accountRepoMock;
        private readonly Mock<IAddressRepository> _addressRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IFixture _fixture; 
        
        public AddressServiceTests()
        {
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _accountRepoMock = new Mock<IAccountRepository>();
            _addressRepoMock = new Mock<IAddressRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _currentUserService = _currentUserServiceMock.Object;
            _accountRepository =  _accountRepoMock.Object;
            _addressRepository = _addressRepoMock.Object;
            _unitOfWork = _unitOfWorkMock.Object;
            _fixture = new Fixture();
            
            _addressService = new AddressService(_currentUserService, _addressRepository, _unitOfWork, _accountRepository);

        }
        #region EditUserAddress Method Tests
        [Fact]
        public async Task Edit_ValidDetails_EditsAddress()
        {
            //Arrange
            Address address = _fixture.Build<Address>()
                .Without(item => item.Country)
                .Without(item => item.User)
                .Create();

            AddressUpdateRequest addressDto = _fixture.Create<AddressUpdateRequest>();
            
            _addressRepoMock.Setup(item => item.GetAddressByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(address);

            _unitOfWorkMock.Setup(item => item.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1);
            
            //Act
            var result = await _addressService.EditUserAddress(addressDto);
            
            //Assert
            result.IsSuccess.Should().BeTrue();
            address.Street.Should().Be(addressDto.Street);
            address.HouseNumber.Should().Be(addressDto.HouseNumber);
            address.CountryId.Should().Be(addressDto.CountryId);
            address.Place.Should().Be(addressDto.Place);
            address.PostalCity.Should().Be(addressDto.PostalCity);
            address.PostalCode.Should().Be(addressDto.PostalCode);
            address.DateEdited.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Edit_AddressNull_ReturnsFailureResult()
        {
            //Arrange
            int invalidAddressId = _fixture.Create<int>();
            AddressUpdateRequest addressDto = _fixture.Create<AddressUpdateRequest>();
            _addressRepoMock.Setup(item => item.GetAddressByIdAsync(invalidAddressId)).ReturnsAsync((Address?)null); 
            
            //Act
            var result = await _addressService.EditUserAddress(addressDto);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AddressErrors.AddressNotFound);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
        #endregion
    }
}
