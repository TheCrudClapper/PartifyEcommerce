using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class ConditionGetterService : IConditionGetterService
    {

        public readonly IConditionRepository _conditionRepo;
        public ConditionGetterService(IConditionRepository conditionRepository)
        {
            _conditionRepo = conditionRepository;
        }
        public async Task<IEnumerable<SelectListItemDto>> GetProductConditionsAsSelectList()
        {
            var conditions = await _conditionRepo.GetAllConditionsAsync();

            return conditions.Select(item => item.ToSelectListItem());
        }

    }
}
