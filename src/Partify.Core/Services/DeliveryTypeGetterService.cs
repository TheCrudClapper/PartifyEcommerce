using CSOS.Core.DTO;
using CSOS.Core.ServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;

namespace CSOS.Core.Services
{
    public class DeliveryTypeGetterService : IDeliveryTypeGetterService
    {
        private readonly IDeliveryTypeRepository _deliveryTypeRepository;

        public DeliveryTypeGetterService(IDeliveryTypeRepository deliveryTypeRepository)
        {
            _deliveryTypeRepository = deliveryTypeRepository;
        }

        public async Task<IEnumerable<SelectListItemDto>> GetAllDeliveryTypesAsSelectionList()
        {
            var types = await _deliveryTypeRepository.GetAllDeliveryTypesAsync();
            return types.Select(item => item.ToSelectListItem()).ToList();

        }

        public async Task<IEnumerable<SelectListItemDto>> GetOtherDeliveryTypes()
        {
            var types = await _deliveryTypeRepository.GetAllDeliveryTypesAsync();

            return types.Where(item => !item.Title.Contains("locker", StringComparison.OrdinalIgnoreCase))
                .Select(item => item.ToSelectListItem()).ToList();
        }

        public async Task<IEnumerable<DeliveryTypeResponse>> GetParcelLockerDeliveryTypes()
        {
            var types = await _deliveryTypeRepository.GetAllDeliveryTypesAsync();

            return types.Where(item => item.Title.Contains("locker", StringComparison.OrdinalIgnoreCase))
                .Select(item => item.ToDeliveryTypeResponseDto()).ToList();
        }
    }
}