using AutoFixture;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.ServiceContracts;
using CSOS.Core.Services;
using FluentAssertions;
using Moq;
namespace CSOS.Tests
{
    public class DeliveryTypeGetterServiceTests
    {
        private readonly IDeliveryTypeRepository  _deliveryTypeRepo;
        private readonly Mock<IDeliveryTypeRepository> _deliveryTypeRepoMock;
        private readonly IDeliveryTypeGetterService _deliveryTypeGetterService;
        private readonly Fixture _fixture;
        public DeliveryTypeGetterServiceTests()
        {
            _deliveryTypeRepoMock = new Mock<IDeliveryTypeRepository>();
            _deliveryTypeRepo = _deliveryTypeRepoMock.Object;
            _deliveryTypeGetterService = new DeliveryTypeGetterService(_deliveryTypeRepo);
            _fixture = new Fixture();
        }
        #region  GetAllDeliveryTypesAsSelectionList Method Tests
        [Fact]
        public async Task GetAllDeliveryTypesAsSelectionList_DeliveryTypesEmpty_ReturnEmptyList()
        {
            //Arrange
            _deliveryTypeRepoMock.Setup(item => item.GetAllDeliveryTypesAsync()).ReturnsAsync(new List<DeliveryType>() { });

            //Act
            var deliveryTypes = await _deliveryTypeGetterService.GetAllDeliveryTypesAsSelectionList();

            //Assert
            deliveryTypes.Should().BeEmpty();
            deliveryTypes.Should().AllBeOfType<DeliveryType>();
        }

        [Fact]
        public async Task GetAllDeliveryTypesAsSelectionList_DeliveryTypesExists_ReturnsDeliveries()
        {
            //Arrange
            List<DeliveryType> deliveries = _fixture.Build<DeliveryType>()
                .Without(item => item.OfferDeliveryTypes)
                .CreateMany()
                .ToList();

            _deliveryTypeRepoMock.Setup(item => item.GetAllDeliveryTypesAsync()).ReturnsAsync(deliveries);

            //Act
            var deliveriesFromService = await _deliveryTypeGetterService.GetAllDeliveryTypesAsSelectionList();

            //Assert
            deliveriesFromService.Should().NotBeEmpty();
            deliveriesFromService.Should().HaveCount(deliveries.Count);
            deliveriesFromService.Should().AllBeOfType<SelectListItemDto>();

        }
        #endregion

        #region GetOtherDeliveryTypes Method Tests
        [Fact]
        public async Task GetOtherDeliveryTypes_DeliveryTypesEmpty_ReturnEmptyList()
        {
            //Arrange
            _deliveryTypeRepoMock.Setup(item => item.GetAllDeliveryTypesAsync()).ReturnsAsync(new List<DeliveryType>() { });

            //Act
            var deliveries = await _deliveryTypeGetterService.GetOtherDeliveryTypes();

            //Assert
            deliveries.Should().BeEmpty();
            deliveries.Should().AllBeOfType<SelectListItemDto>();
        }

        //think about it
        [Fact]
        public async Task GetOtherDeliveryTypes_DeliveryExists_ReturnOtherDeliveries()
        {
            //Arrange
            List<DeliveryType> deliveries = _fixture.Build<DeliveryType>()
                .Without(item => item.OfferDeliveryTypes)
                .CreateMany()
                .ToList();


            List<DeliveryType> parcelLockerDeliveries = _fixture.Build<DeliveryType>()
                .Without(item => item.OfferDeliveryTypes)
                .With(item => item.Title, "locker")
                .CreateMany().ToList();

            deliveries.AddRange(parcelLockerDeliveries);

            _deliveryTypeRepoMock.Setup(item => item.GetAllDeliveryTypesAsync()).ReturnsAsync(deliveries);

            //Act
            var deliveriesFromService = await _deliveryTypeGetterService.GetOtherDeliveryTypes();

            //Assert
            deliveriesFromService.Should().NotBeNull();
            deliveriesFromService.Should().HaveCount(deliveries.Count - parcelLockerDeliveries.Count);
            deliveriesFromService.Should().OnlyContain(item => !item.Text.Contains("locker"));
        }
        #endregion

        #region GetParcelLockerDeliveryTypes Method Tests
        [Fact]
        public async Task GetParcelLockerDeliveryTypes_DeliveryTypesEmpty_ReturnsEmptyList()
        {
            //Arrange
            _deliveryTypeRepoMock.Setup(item => item.GetAllDeliveryTypesAsync()).ReturnsAsync(new List<DeliveryType>() { });

            //Act
            var deliveries = await _deliveryTypeGetterService.GetParcelLockerDeliveryTypes();

            //Assert
            deliveries.Should().BeEmpty();
            deliveries.Should().AllBeOfType<DeliveryTypeResponse>();
        }
        [Fact]
        public async Task GetParcelLockerDeliveryTypes_DeliveryExists_ReturnsDeliveries()
        {
            //Arrange
            List<DeliveryType> deliveries = _fixture.Build<DeliveryType>()
                .Without(item => item.OfferDeliveryTypes)
                .CreateMany()
                .ToList();


            List<DeliveryType> parcelLockerDeliveries = _fixture.Build<DeliveryType>()
                .Without(item => item.OfferDeliveryTypes)
                .With(item => item.Title, "locker")
                .CreateMany().ToList();

            deliveries.AddRange(parcelLockerDeliveries);

            _deliveryTypeRepoMock.Setup(item => item.GetAllDeliveryTypesAsync()).ReturnsAsync(deliveries);

            //Act
            var deliveriesFromService = await _deliveryTypeGetterService.GetParcelLockerDeliveryTypes();

            //Assert
            deliveriesFromService.Should().NotBeNull();
            deliveriesFromService.Should().HaveCount(deliveries.Count - parcelLockerDeliveries.Count);
            deliveriesFromService.Should().OnlyContain(item => item.Title.Contains("locker"));
        }
        #endregion
    }
}

