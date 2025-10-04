using CSOS.Core.Caching;
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

    public async Task<IEnumerable<SelectListItemDto>> GetAllDeliveryTypesAsSelectList(CancellationToken cancellationToken)
    {
        var deliveries = await GetAllDeliveryTypesCachedAsDto(cancellationToken);

        return deliveries.Select(x => x.ToSelectListItem());
    }

    public async Task<IEnumerable<SelectListItemDto>> GetOtherDeliveryTypes(CancellationToken cancellationToken)
    {
        var deliveriesDto = await GetAllDeliveryTypesCachedAsDto(cancellationToken);

        return deliveriesDto.Where(item => !item.Title.Contains("locker", StringComparison.OrdinalIgnoreCase))
            .Select(item => item.ToSelectListItem());
    }

    public async Task<IEnumerable<DeliveryTypeResponse>> GetParcelLockerDeliveryTypes(CancellationToken cancellationToken)
    {
        var deliveriesDto = await GetAllDeliveryTypesCachedAsDto(cancellationToken);

        return deliveriesDto.Where(item => item.Title.Contains("locker", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Retrieves all delivery types as DTOs from cache or repository.
    /// Materializes as a List for safe caching.
    /// </summary>
    private async Task<List<DeliveryTypeResponse>> GetAllDeliveryTypesCachedAsDto(CancellationToken cancellationToken)
    {
        var objFromCache = await _cachingHelper
            .GetCachedObject<List<DeliveryTypeResponse>>(CacheKeyAllDeliveryTypes, cancellationToken);
        if(objFromCache.Found)
            return objFromCache.Value!;

        var deliveries = await _deliveryTypeRepository.GetAllDeliveryTypesAsync(cancellationToken);

        var dtos = deliveries.Select(item => item.ToDeliveryTypeResponseDto())
            .ToList();

        await _cachingHelper
            .CacheObject(dtos, CacheKeyAllDeliveryTypes, CachingProfiles.LongTTLCacheOption, cancellationToken);
        return dtos;
    }

}