using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.OfferDto;
using CSOS.UI.ViewModels.DeliveryTypeViewModels;
using CSOS.UI.ViewModels.OfferViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class OfferViewModelMappings
    {
        public static EditOfferViewModel ToEditOfferViewModel(this EditOfferResponse dto, IPictureHandlerService pictureHandlerService)
        {
            return new EditOfferViewModel
            {
                Description = dto.Description,
                Price = dto.Price,
                ProductName = dto.ProductName,
                Id = dto.Id,
                IsOfferPrivate = dto.IsOfferPrivate,
                SelectedOtherDeliveries = dto.SelectedOtherDeliveries,
                SelectedParcelLocker = dto.SelectedParcelLocker,
                SelectedProductCondition = dto.SelectedProductCondition,
                StockQuantity = dto.StockQuantity,
                SelectedProductCategory = dto.SelectedProductCategory,
                ExistingImagesUrls = dto.ExistingImagesUrls!.Select(item => new SelectListItem
                {
                    Text = item.Text,
                    Value = pictureHandlerService.ReplaceImageIfNotFound(item.Value)
                })
                .ToList(),
                ExistingImagesCount = dto.ExistingImagesUrls?.Count ?? 0,
            };
        }

        public static OfferIndexItemViewModel ToOfferIndexItemViewModel(this OfferIndexResponse dto, IPictureHandlerService pictureHandlerService)
        {
            return new OfferIndexItemViewModel
            {
                Description = dto.Description,
                Price = dto.Price,
                ProductName = dto.ProductName,
                Id = dto.Id,
                DateCreated = dto.DateCreated,
                ImageUrl = pictureHandlerService.ReplaceImageIfNotFound(dto.ImageUrl),
                ProductCategory = dto.ProductCategory,
                ProductCondition = dto.ProductCondition,
                StockQuantity = dto.StockQuantity,
                Seller = dto.Seller,
            };
        }
        
        public static OfferDetailsViewModel ToOfferDetailsViewModel(this OfferResponse dto, IPictureHandlerService pictureHandlerService)
        {
            
            return new OfferDetailsViewModel
            {
                AvaliableDeliveryTypes = dto.AvaliableDeliveryTypes.Select(item => new DeliveryTypeViewModel
                {
                    Price = item.Price,
                    Title = item.Title,
                    Id = item.Id,
                }).ToList(),
                ProductCondition = dto.ProductCondition,
                Id = dto.Id,
                IsLiked = dto.IsLiked,
                Title = dto.Title,
                Price = dto.Price,
                DateCreated = dto.DateCreated,
                Description = dto.Description,
                Place = dto.Place,
                PostalCity = dto.PostalCity,
                PostalCode = dto.PostalCode,
                Seller= dto.Seller,
                ProductCategory = dto.ProductCategory,
                StockQuantity = dto.StockQuantity,
                ProductImages = dto.ProductImages.Select(img => pictureHandlerService.ReplaceImageIfNotFound(img)).ToList(),
            };
        }

        public static UserOffersViewModel ToUserOffersViewModel(this UserOfferResponse dto, IPictureHandlerService pictureHandlerService)
        {
            return new UserOffersViewModel
            {
                DateCreated = dto.DateCreated,
                Id = dto.Id,
                ImageUrl = pictureHandlerService.ReplaceImageIfNotFound(dto.ImageUrl),
                Price = dto.Price,
                ProductCategory = dto.ProductCategory,
                ProductCondition = dto.ProductCondition,
                ProductName = dto.ProductName,
                IsOfferPrivate = dto.IsOfferPrivate,
                StockQuantity = dto.StockQuantity,
            };
        }
    }
}
