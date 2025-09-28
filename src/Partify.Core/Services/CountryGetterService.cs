using CSOS.Core.Caching;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.CountryDto;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services;

public class CountryGetterService : ICountriesGetterService
{
    private readonly ICountryRepository _countryRepository;
    private readonly ICachingHelper _cachingHelper;
    private const string CacheKeyAllCountries = "countries-all";

    public CountryGetterService(ICountryRepository countryRepository, ICachingHelper cachingHelper)
    {
        _countryRepository = countryRepository;
        _cachingHelper = cachingHelper;
    }

    public async Task<IEnumerable<SelectListItemDto>> GetCountriesSelectionList()
    {
        var countriesDto = await GetAllCountriesCachedAsDto();

        var dtos = countriesDto.Select(item => item.ToSelectListItem());

        return dtos;
    }


    /// <summary>
    /// Retrieves all countries as DTOs from cache or repository.
    /// Materializes as a List for safe caching.
    /// </summary>
    public async Task<List<CountryResponse>> GetAllCountriesCachedAsDto()
    {
        var objFromCache = await _cachingHelper.GetCachedObject<List<CountryResponse>>(CacheKeyAllCountries);
        if (objFromCache.Found)
            return objFromCache.Value!;

        var countries = await _countryRepository.GetAllCountriesAsync();

        var dtos = countries.Select(item => item.ToCountryReponse()).ToList();

        await _cachingHelper.CacheObject(dtos,CacheKeyAllCountries, CachingProfiles.LongTTLCacheOption);
        return dtos;
    }
}