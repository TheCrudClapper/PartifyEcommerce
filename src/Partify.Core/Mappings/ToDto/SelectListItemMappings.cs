using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class SelectListItemMappings
    {
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
