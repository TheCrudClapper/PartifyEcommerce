using CSOS.Core.Caching;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.ProductImage;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services;

public class ProductImageService : IProductImageService
{
    private readonly IProductImageRepository _productImageRepo;
    private readonly ICachingHelper _cachingHelper;

    public ProductImageService(IProductImageRepository productImageRepository, ICachingHelper cachingHelper)
    {
        _productImageRepo = productImageRepository;
        _cachingHelper = cachingHelper;
    }

    public async Task<IEnumerable<SelectListItemDto>> GetOfferPicturesAsync(int offerId)
    {
        var items = await _productImageRepo.GetImagesFromOfferAsync(offerId);

        return items.Select(item => item.ToSelectListItem()).ToList();
    }
    
    public Result DeleteImagesFromOffer(IEnumerable<ProductImage> images, IEnumerable<int>? imageIds)
    {
        if (!images.Any())
            return Result.Failure(ProductImageErrors.ProductImagesAreEmpty);

        if(imageIds != null)
        {
            foreach (var imageToDelete in images)
            {
                if (imageIds.Contains(imageToDelete.Id))
                {
                    imageToDelete.DateDeleted = DateTime.UtcNow;
                    imageToDelete.IsActive = false;
                }
            }
        }

        return Result.Success();
    }
}
