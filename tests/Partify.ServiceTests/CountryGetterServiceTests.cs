using AutoFixture;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Services;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.ServiceContracts;
using CSOS.Core.Services;
using FluentAssertions;
using Moq;

namespace CSOS.Tests
{
    public class CountryGetterServiceTests
    {
        private readonly ICountriesGetterService _countryGetterService;
        private readonly ICountryRepository _countryRepo;
        private readonly Mock<ICountryRepository> _countryRepoMock;
        private readonly Fixture _fixture;
        
        public CountryGetterServiceTests()
        {
            _countryRepoMock = new Mock<ICountryRepository>();
            _countryRepo = _countryRepoMock.Object;
            _countryGetterService = new CountryGetterService(_countryRepo);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetCountriesSelectionList_CountriesEmpty_ReturnEmptyList()
        {
            //Arrange
            _countryRepoMock.Setup(item => item.GetAllCountriesAsync()).ReturnsAsync(new List<Country>() {});
            
            //Act
            var countriesFromService = await _countryGetterService.GetCountriesSelectionList();
            
            //Assert
            countriesFromService.Should().BeEmpty();
            countriesFromService.Should().AllBeOfType<SelectListItemDto>();
        }
        
        
        [Fact]
        public async Task GetCountriesSelectionList_CountriesExist_ReturnCountriesAsSelectionList()
        {
            //Arrange
            List<Country> countries = _fixture.Build<Country>()
                .Without(item => item.Addresses)
                .CreateMany().ToList();
            
            _countryRepoMock.Setup(item => item.GetAllCountriesAsync()).ReturnsAsync(countries);
            
            //Act
            var countriesFromService = await _countryGetterService.GetCountriesSelectionList();
            
            //Assert
            countriesFromService.Should().NotBeEmpty();
            countriesFromService.Should().HaveCount(countries.Count);
            countriesFromService.Should().AllBeOfType<SelectListItemDto>();
        }
    }
}

