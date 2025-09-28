using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.CategoryDto;
using CSOS.Core.DTO.Condition;
using CSOS.Core.DTO.CountryDto;
using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class SelectListItemMappings
    {

        public static SelectListItemDto ToSelectListItem(this CountryResponse dto)
        {
            return new SelectListItemDto
            {
                Text = dto.CountryName,
                Value = dto.Id.ToString(),
            };
        }
        public static SelectListItemDto ToSelectListItem(this ConditionResponse dto)
        {
            return new SelectListItemDto
            {
                Text = dto.ConditionTitle,
                Value = dto.Id.ToString(),
            };
        }

        public static SelectListItemDto ToSelectListItem(this CategoryResponse dto)
        {
            return new SelectListItemDto
            {
                Text = dto.Name,
                Value = dto.Id.ToString()
            };
        }
        public static SelectListItemDto ToSelectListItem(this DeliveryTypeResponse dto)
        {
            return new SelectListItemDto
            {
                Text = dto.Title,
                Value = dto.Id.ToString()
            };
        }
        public static SelectListItemDto ToSelectListItem(this DeliveryType deliveryType)
        {
            return new SelectListItemDto
            {
                Text = deliveryType.Title,
                Value = deliveryType.Id.ToString()
            };
        }

        public static SelectListItemDto ToSelectListItem(this ProductImage productImage)
        {
            return new SelectListItemDto
            {
                Value = productImage.ImagePath,
                Text = productImage.ImagePath,
            };
        }

        public static SelectListItemDto ToSelectListItem(this ProductCategory productCategory)
        {
            return new SelectListItemDto
            {
                Text = productCategory.Name,
                Value = productCategory.Id.ToString(),
            };
        }

        public static SelectListItemDto ToSelectListItem(this Condition condition)
        {
            return new SelectListItemDto
            {
                Text = condition.ConditionTitle,
                Value = condition.Id.ToString(),
            };
        }

        public static SelectListItemDto ToSelectListItem(this Country country)
        {
            return new SelectListItemDto
            {
                Text = country.CountryName,
                Value = country.Id.ToString()
            };
        }
    }
}
