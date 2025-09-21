using Xunit;
using Moq;
using AutoFixture;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.ServiceContracts;
using CSOS.Core.Services;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.OfferDto;
using CSOS.Core.DTO.UniversalDto;
using FluentAssertions;
using Xunit.Abstractions;

namespace CSOS.Tests
{
    public class CategoryGetterServiceTests
    {
        private readonly IProductCategoryRepository _productCategoryRepo;
        private readonly Mock<IProductCategoryRepository> _productCategoryMoq;
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Fixture _fixture;
        public CategoryGetterServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();
            _productCategoryMoq = new Mock<IProductCategoryRepository>();
            _productCategoryRepo = _productCategoryMoq.Object;
            _categoryGetterService = new CategoryGetterService(_productCategoryRepo);


            _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetProductCategoriesAsMainPageCardResponseDto_CategoriesExists_ReturnsCategories()
        {
            //Arrange
            List<ProductCategory> categories = _fixture.CreateMany<ProductCategory>().ToList();

            _productCategoryMoq.Setup(item => item.GetAllProductCategoriesAsync()).ReturnsAsync((categories));

            //Act
            var categoriesFromDb = await _categoryGetterService.GetProductCategoriesAsCardResponse();

            //Assert
            categoriesFromDb.Should().NotBeNull();
            categoriesFromDb.Should().HaveCount(categories.Count);
            categoriesFromDb.Should().AllBeOfType<CardResponse>();

        }

        [Fact]
        public async Task GetProductCategoriesAsMainPageCardResponseDto_CategoriesEmpty_ReturnsEmptyList()
        {
            //Arrange
            List<ProductCategory> categories = new List<ProductCategory>();
            _productCategoryMoq.Setup(item => item.GetAllProductCategoriesAsync()).ReturnsAsync((categories));

            //Act
            var categoriesFromDb = await _categoryGetterService.GetProductCategoriesAsCardResponse();

            //Assert
            categoriesFromDb.Should().NotBeNull();
            categoriesFromDb.Should().BeEmpty();
            categoriesFromDb.Should().AllBeOfType<CardResponse>();
        }
    }
}
