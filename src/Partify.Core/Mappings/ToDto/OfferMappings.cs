using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.Core.DTO.OfferDto;
using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class OfferMappings
    {
        public static CardResponse ToCardResponse(this Offer offer)
        {
            var firstActiveImage = offer.Product.ProductImages.FirstOrDefault(item => item.IsActive);
            return new CardResponse()
            {
                Id = offer.Id,
                ImageUrl = firstActiveImage?.ImagePath,
                Price = offer.Price,
                Title = offer.Product.ProductName,
            };
        }
        
        public static OfferResponse ToOfferResponse(this Offer offer, Guid? currentUserId)
        {
            return new OfferResponse()
            {
                Id = offer.Id,
                ProductCondition = offer.Product.Condition.ConditionTitle,
                DateCreated = offer.DateCreated.Date,
                IsLiked = currentUserId.HasValue && offer.LikedOffers
                    .Any(lo => lo.UserId == currentUserId && lo.OfferId ==offer.Id && lo.IsActive),
                Seller = offer.Seller.UserName!,
                Description = offer.Product.Description,
                ProductCategory = offer.Product.ProductCategory.Name,
                Price = offer.Price,
                StockQuantity = offer.StockQuantity,
                IsSellerCompany = offer.Seller.NIP != null,
                Title = offer.Product.ProductName,
                Place = offer.Seller.Address.Place,
                PostalCity = offer.Seller.Address.PostalCity,
                PostalCode = offer.Seller.Address.PostalCode,
                ProductImages = offer.Product.ProductImages
                        .Where(item => item.IsActive)
                        .Select(item => item.ImagePath)
                        .ToList(),
                AvaliableDeliveryTypes = offer.OfferDeliveryTypes
                    .Select(item => new DeliveryTypeResponse()
                    {
                        Title = item.DeliveryType.Title,
                        Price = item.DeliveryType.Price,
                        Id = item.DeliveryType.Id
                    }).ToList()
            };
        }

        public static UserOfferResponse ToUserOfferResponse(this Offer offer)
        {
            var activeImage = offer.Product.ProductImages.FirstOrDefault(img => img.IsActive);
            
            return new UserOfferResponse
            {
                Id = offer.Id,
                ProductName = offer.Product.ProductName,
                Price = offer.Price,
                ImageUrl = activeImage?.ImagePath,
                IsOfferPrivate = offer.IsOfferPrivate,
                ProductCategory = offer.Product.ProductCategory.Name,
                ProductCondition = offer.Product.Condition.ConditionTitle,
                DateCreated = offer.DateCreated,
                StockQuantity = offer.StockQuantity,
            };
        }

        public static OfferIndexResponse ToOfferIndexResponse(this Offer offer)
        {
            var activeImage = offer.Product.ProductImages.FirstOrDefault(img => img.IsActive);
            return new OfferIndexResponse()
            {
                Id = offer.Id,
                ProductName = offer.Product.ProductName,
                ProductCategory = offer.Product.ProductCategory.Name,
                ProductCondition = offer.Product.Condition.ConditionTitle,
                DateCreated = offer.DateCreated.Date,
                Price = offer.Price,
                Seller = offer.Seller.UserName!,
                Description = offer.Product.Description,
                StockQuantity = offer.StockQuantity,
                ImageUrl = activeImage?.ImagePath
            };
        }
        
        public static EditOfferResponse ToEditOfferResponse(this Offer offer)
        {
            return new EditOfferResponse()
            {
                Id = offer.Id,
                ProductName = offer.Product.ProductName,
                Price = offer.Price,
                ExistingImagesUrls = offer.Product.ProductImages
                  .Where(item => item.IsActive)
                  .Select(item => new SelectListItemDto()
                  {
                      Value = item.Id.ToString(),
                      Text = item.ImagePath,
                  })
                  .ToList(),
                Description = offer.Product.Description,
                SelectedProductCondition = offer.Product.Condition.Id.ToString(),
                SelectedProductCategory = offer.Product.ProductCategory.Id.ToString(),
                IsOfferPrivate = offer.IsOfferPrivate,
                StockQuantity = offer.StockQuantity,
                SelectedOtherDeliveries = offer.OfferDeliveryTypes
                  .Where(item => !item.DeliveryType.Title.Contains("Locker"))
                  .Select(item => item.DeliveryTypeId)
                  .ToList(),
                SelectedParcelLocker = offer.OfferDeliveryTypes
                  .Where(item => item.DeliveryType.Title.Contains("Locker"))
                  .Select(item => item.DeliveryTypeId)
                  .FirstOrDefault(),
            };
        }

        public static CardResponse ToCardResponse(this ProductCategory productCategory)
        {
            return new CardResponse()
            {
                Id = productCategory.Id,
                ImageUrl = productCategory.CategoryImage,
                Title = productCategory.Name,
            };
        }
    }
}
