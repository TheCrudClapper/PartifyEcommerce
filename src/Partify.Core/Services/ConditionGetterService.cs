using CSOS.Core.Caching;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;
using System.Collections.Generic;

namespace CSOS.Core.Services;

public class ConditionGetterService : IConditionGetterService
{

    public readonly IConditionRepository _conditionRepo;
    private readonly ICachingHelper _cachingHelper;
    private const string CacheKeySelectList = "conditions-as-select-list:all";
    public ConditionGetterService(IConditionRepository conditionRepository, ICachingHelper cachingHelper)
    {
        _conditionRepo = conditionRepository;
        _cachingHelper = cachingHelper;
    }
    public async Task<IEnumerable<SelectListItemDto>> GetProductConditionsAsSelectList()
    {
        var objFromCache = await _cachingHelper.GetCachedObject<IEnumerable<SelectListItemDto>>(CacheKeySelectList);
        if (objFromCache.Found)
            return objFromCache.Value!;

        var conditions = await _conditionRepo.GetAllConditionsAsync();
        var dtos = conditions.Select(item => item.ToSelectListItem()).ToList();

        await _cachingHelper.CacheObject(dtos, CacheKeySelectList, CachingProfiles.LongTTLCacheOption);
        return dtos;
    }

}
