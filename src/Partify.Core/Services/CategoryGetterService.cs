using CSOS.Core.Caching;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;
namespace CSOS.Core.Services;

public class CategoryGetterService : ICategoryGetterService
{
    private readonly IProductCategoryRepository _productCategoryRepo;
    private readonly ICachingHelper _cachingHelper;

    //Cache key constants
    private const string CacheKeyCardResponse = "categories-as-card-response:all";
    private const string CacheKeySelectList = "categories-as-select-list:all";
    public CategoryGetterService(IProductCategoryRepository productCategoryRepository, ICachingHelper cachingHelper)
    {
        _productCategoryRepo = productCategoryRepository;
        _cachingHelper = cachingHelper;
    }

    public async Task<IEnumerable<CardResponse>> GetProductCategoriesAsCardResponse()
    {
        var objFromCache = await _cachingHelper.GetCachedObject<IEnumerable<CardResponse>>(CacheKeyCardResponse);
        if (objFromCache.Found)
            return objFromCache.Value!;

        var categories = await _productCategoryRepo.GetAllProductCategoriesAsync();

        var dtos = categories.Select(item => item.ToCardResponse()).ToList();

        await _cachingHelper.CacheObject(dtos, CacheKeyCardResponse, CachingProfiles.ShortTTLCacheOption);
        return dtos;
    }
    public async Task<IEnumerable<SelectListItemDto>> GetProductCategoriesAsSelectList()
    {
        var objFromCache = await _cachingHelper.GetCachedObject<IEnumerable<SelectListItemDto>>(CacheKeySelectList);
        if (objFromCache.Found)
            return objFromCache.Value!;

        var categories = await _productCategoryRepo.GetAllProductCategoriesAsync();

        var dtos = categories.Select(item => item.ToSelectListItem()).ToList();

        await _cachingHelper.CacheObject(dtos, CacheKeySelectList, CachingProfiles.MediumTTLCacheOption);
        return dtos;
    }
}
