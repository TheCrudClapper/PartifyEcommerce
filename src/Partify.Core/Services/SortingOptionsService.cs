using CSOS.Core.DTO;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class SortingOptionsService : ISortingOptionService
    {
        //Just for testing
        //Later create table in db for filtering options
        public IEnumerable<SelectListItemDto> GetSortingOptions()
        {
            return new List<SelectListItemDto>()
            {
                new SelectListItemDto()
                {
                    Text = "Price - from highest",
                    Value = "price_desc",
                },
                new SelectListItemDto()
                {
                    Text = "Price - from lowest",
                    Value = "price_asc",
                }
            };
        }
    }
}
