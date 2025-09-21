using AutoFixture;
using ComputerServiceOnlineShop.Entities.Models;
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
    public class ConditionGetterCategoryServiceTests
    {
        private readonly IConditionGetterService _conditionGetterService;
        private readonly IConditionRepository _conditionRepo;
        private readonly Mock<IConditionRepository> _conditionRepoMock;
        private readonly Fixture _fixture;
        public ConditionGetterCategoryServiceTests()
        {
            _fixture = new Fixture();
            _conditionRepoMock = new Mock<IConditionRepository>();
            _conditionRepo = _conditionRepoMock.Object;
            _conditionGetterService = new ConditionGetterService(_conditionRepo);

            _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetProductConditionsAsSelectList_CategoriesEmpty_ReturnsEmptyList()
        {
            //Arrange
            _conditionRepoMock.Setup(item => item.GetAllConditionsAsync())
                .ReturnsAsync(new List<Condition>() { });

            //Act
            var conditionsFromService = await _conditionGetterService.GetProductConditionsAsSelectList();

            //Assert
            conditionsFromService.Should().BeEmpty();
            conditionsFromService.Should().AllBeOfType<SelectListItemDto>();
        }

        [Fact]
        public async Task GetProductConditionsAsSelectList_CategoriesExists_ReturnsCategoriesAsSelectList()
        {
            //Arrange
            List<Condition> conditions = _fixture.CreateMany<Condition>().ToList();

            _conditionRepoMock.Setup(item => item.GetAllConditionsAsync()).ReturnsAsync(conditions);

            //Act
            var conditionsFromService = await _conditionGetterService.GetProductConditionsAsSelectList();

            //Assert
            conditionsFromService.Should().NotBeEmpty();
            conditionsFromService.Should().HaveCount(conditions.Count);
            conditionsFromService.Should().AllBeOfType<SelectListItemDto>();
        }

        [Fact]
        public async Task GetProductConditionsAsSelectList_CategoriesNull_ReturnsEmptyList()
        {
            //Arrange
            List<Condition> conditions = _fixture.CreateMany<Condition>().ToList();

            _conditionRepoMock.Setup(item => item.GetAllConditionsAsync()).ReturnsAsync(conditions);

            //Act
            var conditionsFromService = await _conditionGetterService.GetProductConditionsAsSelectList();

            //Assert
            conditionsFromService.Should().NotBeEmpty();
            conditionsFromService.Should().HaveCount(conditions.Count);
            conditionsFromService.Should().AllBeOfType<SelectListItemDto>();
        }
    }
}
