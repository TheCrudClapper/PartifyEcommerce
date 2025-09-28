using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.CategoryDto;

namespace CSOS.Core.Mappings.ToDto;

public static class ProductCategoryMappings
{
    public static CategoryResponse ToCategoryResponse(this ProductCategory category)
    {
        return new CategoryResponse(
            category.Id,
            category.Name,
            category.Description,
            category.CategoryImage
            );
    }


}
