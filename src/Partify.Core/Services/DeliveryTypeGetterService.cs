using CSOS.Core.Caching;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services;

public class DeliveryTypeGetterService : IDeliveryTypeGetterService
{
    private readonly IDeliveryTypeRepository _deliveryTypeRepository;
    private readonly ICachingHelper _cachingHelper;
    private const string CacheKeyAllDeliveryTypes = "delivery-types:all";

    public DeliveryTypeGetterService(IDeliveryTypeRepository deliveryTypeRepository, ICachingHelper cachingHelper)
    {
        _deliveryTypeRepository = deliveryTypeRepository;
        _cachingHelper = cachingHelper; 
    }

    public async Task<IEnumerable<SelectListItemDto>> GetAllDeliveryTypesAsSelectionList()
    {
        var deliveries = await GetAllDeliveryTypesCached();

        return deliveries.Select(x => x.ToSelectListItem())
            .ToList();
    }

    public async Task<IEnumerable<SelectListItemDto>> GetOtherDeliveryTypes()
    {
        var deliveries = await GetAllDeliveryTypesCached();

        return deliveries.Where(item => !item.Title.Contains("locker", StringComparison.OrdinalIgnoreCase))
            .Select(item => item.ToSelectListItem())
            .ToList();
    }

    public async Task<IEnumerable<DeliveryTypeResponse>> GetParcelLockerDeliveryTypes()
    {
        var deliveries = await GetAllDeliveryTypesCached(); ;

        return deliveries.Where(item => item.Title.Contains("locker", StringComparison.OrdinalIgnoreCase))
            .Select(item => item.ToDeliveryTypeResponseDto())
            .ToList();
    }

    private async Task<IEnumerable<DeliveryType>> GetAllDeliveryTypesCached()
    {
        var objFromCache = await _cachingHelper.GetCachedObject<IEnumerable<DeliveryType>>(CacheKeyAllDeliveryTypes);
        if(objFromCache.Found)
            return objFromCache.Value!;

        var deliveries = await _deliveryTypeRepository.GetAllDeliveryTypesAsync();
        await _cachingHelper.CacheObject(deliveries, CacheKeyAllDeliveryTypes, CachingProfiles.LongTTLCacheOption);
        return deliveries;
    }

}