using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class CategoryGetterService : ICategoryGetterService
    {
        private readonly IProductCategoryRepository _productCategoryRepo;
        public CategoryGetterService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepo = productCategoryRepository;
        }

        public async Task<IEnumerable<CardResponse>> GetProductCategoriesAsCardResponse()
        {
            var categories = await _productCategoryRepo.GetAllProductCategoriesAsync();

            return categories.Select(item => new CardResponse()
            {
                Id = item.Id,
                ImageUrl = item.CategoryImage,
                Title = item.Name,
            });
        }
        public async Task<IEnumerable<SelectListItemDto>> GetProductCategoriesAsSelectList()
        {
            var categories = await _productCategoryRepo.GetAllProductCategoriesAsync();

            return categories.Select(item => item.ToSelectListItem());
        }
    }
}
