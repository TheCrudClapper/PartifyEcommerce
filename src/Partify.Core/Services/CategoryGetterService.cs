using CSOS.Core.Caching;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.CategoryDto;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;
namespace CSOS.Core.Services;

public class CategoryGetterService : ICategoryGetterService
{
    private readonly IProductCategoryRepository _productCategoryRepo;
    private readonly ICachingHelper _cachingHelper;

    //Cache key constants
    private const string CacheKeyAllCategories = "categories:all";
    public CategoryGetterService(IProductCategoryRepository productCategoryRepository, ICachingHelper cachingHelper)
    {
        _productCategoryRepo = productCategoryRepository;
        _cachingHelper = cachingHelper;
    }

    public async Task<IEnumerable<CardResponse>> GetProductCategoriesAsCardResponse(CancellationToken cancellationToken)
    {
        var categoryDtos = await GetAllCategoriesAsDto(cancellationToken);

        return categoryDtos.Select(item => item.ToCardResponse());
    }
    public async Task<IEnumerable<SelectListItemDto>> GetProductCategoriesAsSelectList(CancellationToken cancellationToken)
    {
        var categoryDtos = await GetAllCategoriesAsDto(cancellationToken);

        return categoryDtos.Select(item => item.ToSelectListItem());
    }

    /// <summary>
    /// Retrieves all categories as DTOs from cache or repository.
    /// Materializes as a List for safe caching.
    /// </summary>
    private async Task<List<CategoryResponse>> GetAllCategoriesAsDto(CancellationToken cancellationToken)
    {
        var objFromCache = await _cachingHelper
            .GetCachedObject<List<CategoryResponse>>(CacheKeyAllCategories, cancellationToken);
        if (objFromCache.Found)
            return objFromCache.Value!;

        var categories = await _productCategoryRepo.GetAllProductCategoriesAsync(cancellationToken);

        var dtos = categories.Select(item => item.ToCategoryResponse())
            .ToList();

        await _cachingHelper
            .CacheObject(dtos,CacheKeyAllCategories, CachingProfiles.LongTTLCacheOption, cancellationToken);
        return dtos;

    }
}
