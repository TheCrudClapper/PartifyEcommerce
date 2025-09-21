using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.UniversalDto;
using CSOS.UI.ViewModels.HomePageViewModels;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class MainPageCardMappings
    {
        public static MainPageCardViewModel ToCardViewModel(this CardResponse dto, IPictureHandlerService pictureHandlerService)
        {
            return new MainPageCardViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Price = dto.Price,
                ImageUrl = pictureHandlerService.ReplaceImageIfNotFound(dto.ImageUrl),
            };
        }
    }
}
