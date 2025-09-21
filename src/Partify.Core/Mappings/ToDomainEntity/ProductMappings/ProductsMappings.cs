using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.OfferDto;

namespace CSOS.Core.Mappings.ToDomainEntity.ProductMappings
{
    public static class ProductsMappings
    {
        public static Product ToProductEntity(this OfferAddRequest dto, Offer offer)
        {
            return new Product
            {
                Offer = offer,
                ProductName = dto.ProductName,
                Description = dto.Description,
                ConditionId = dto.SelectedProductCondition,
                ProductCategoryId = dto.SelectedProductCategory,
                IsActive = true,
                DateCreated = DateTime.Now,

                ProductImages = dto.UploadedImagesUrls != null
                ? dto.UploadedImagesUrls.Select(imageUrl => new ProductImage()
                {
                    DateCreated = DateTime.Now,
                    ImagePath = imageUrl,
                    IsActive = true
                }).ToList()
                : new List<ProductImage>()
            };
        }
    }
}
