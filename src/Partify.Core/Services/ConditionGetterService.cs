using CSOS.Core.Caching;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.Condition;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services;

public class ConditionGetterService : IConditionGetterService
{
    private readonly IConditionRepository _conditionRepo;
    private readonly ICachingHelper _cachingHelper;
    private const string CacheKeyAllConditions = "conditions:all";

    public ConditionGetterService(IConditionRepository conditionRepository, ICachingHelper cachingHelper)
    {
        _conditionRepo = conditionRepository;
        _cachingHelper = cachingHelper;
    }

    public async Task<IEnumerable<SelectListItemDto>> GetProductConditionsAsSelectList(CancellationToken cancellationToken)
    {
        var categoryDtos =  await GetAllConditionsCachedAsDto(cancellationToken);
        return categoryDtos
            .Select(item => item.ToSelectListItem());
    }

    /// <summary>
    /// Retrieves all conditions as DTOs from cache or repository.
    /// Materializes as a List for safe caching.
    /// </summary>
    private async Task<List<ConditionResponse>> GetAllConditionsCachedAsDto(CancellationToken cancellationToken)
    {
        var objFromCache = await _cachingHelper
            .GetCachedObject<List<ConditionResponse>>(CacheKeyAllConditions, cancellationToken);
        if (objFromCache.Found)
            return objFromCache.Value!;

        var conditions = await _conditionRepo.GetAllConditionsAsync(cancellationToken);

        var dtos = conditions.Select(item => item.ToConditionResponse())
            .ToList();

        await _cachingHelper
            .CacheObject(dtos, CacheKeyAllConditions, CachingProfiles.LongTTLCacheOption, cancellationToken);
        return dtos;
    }
}
