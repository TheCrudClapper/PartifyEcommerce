﻿using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.ErrorHandling;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepo;
        public ProductImageService(IProductImageRepository productImageRepository)
        {
            _productImageRepo = productImageRepository;
        }

        public async Task<IEnumerable<SelectListItemDto>> GetOfferPicturesAsync(int offerId)
        {
            var items = await _productImageRepo.GetImagesFromOfferAsync(offerId);

            return items.Select(item => item.ToSelectListItem()).ToList();
        }
        
        public Result DeleteImagesFromOffer(IEnumerable<ProductImage> images, IEnumerable<string> imageUrls)
        {
            if (!images.Any())
                return Result.Failure(ProductImageErrors.ProductImagesAreEmpty);

            foreach (var imageToDelete in images)
            {
                if (imageUrls.Any(url => imageToDelete.ImagePath.Contains(url, StringComparison.OrdinalIgnoreCase)))
                {
                    imageToDelete.DateDeleted = DateTime.UtcNow;
                    imageToDelete.IsActive = false;
                }
            }
            return Result.Success();
        }
    }
}
