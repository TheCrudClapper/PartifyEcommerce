using CSOS.Core.Caching;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services;

public class CountryGetterService : ICountriesGetterService
{
    private readonly ICountryRepository _countryRepository;
    private readonly ICachingHelper _cachingHelper;
    private const string CacheKeySelectList = "countries:all";

    public CountryGetterService(ICountryRepository countryRepository, ICachingHelper cachingHelper)
    {
        _countryRepository = countryRepository;
        _cachingHelper = cachingHelper;
    }

    public async Task<IEnumerable<SelectListItemDto>> GetCountriesSelectionList()
    {
        var objFromCache = await _cachingHelper.GetCachedObject<IEnumerable<SelectListItemDto>>(CacheKeySelectList);
        if (objFromCache.Found)
            return objFromCache.Value!;

        var countries = await _countryRepository.GetAllCountriesAsync();

        var dtos =  countries.Select(item => item.ToSelectListItem()).ToList();

        await _cachingHelper.CacheObject(dtos, CacheKeySelectList, CachingProfiles.LongTTLCacheOption);
        return dtos;
    }
}