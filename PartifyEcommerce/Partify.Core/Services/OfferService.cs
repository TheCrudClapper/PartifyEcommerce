using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.DtoContracts;
using CSOS.Core.DTO.OfferDto;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.Helpers;
using CSOS.Core.Mappings.ToDomainEntity.OfferDeliveryTypeMappings;
using CSOS.Core.Mappings.ToDomainEntity.OfferMappings;
using CSOS.Core.Mappings.ToDomainEntity.ProductMappings;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepo;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly IProductImageService _productImageService;
        private readonly IProductRepository _productRepo;
        private readonly IOfferDeliveryTypeRepository _offerDeliveryTypeRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public OfferService(
            IDeliveryTypeGetterService deliveryTypeGetterService,
            IUnitOfWork unitOfWork,
            IOfferRepository offerRepo,
            IProductRepository productRepo,
            IOfferDeliveryTypeRepository offerDeliveryTypeRepo,
            ICurrentUserService currentUserService,
            IPictureHandlerService pictureHandlerService,
            IProductImageService productImageService,
            ISortingOptionService sortingOptionService
            )
        {
            _productRepo = productRepo;
            _offerDeliveryTypeRepo = offerDeliveryTypeRepo;
            _offerRepo = offerRepo;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _pictureHandlerService = pictureHandlerService;
            _productImageService = productImageService;
        }
        public async Task<Result> Add(OfferAddRequest? addRequest)
        {
            if (addRequest == null)
                return Result.Failure(OfferErrors.OfferIsNull);

            Guid userId = _currentUserService.GetUserId();

            Offer offer = addRequest.ToOfferEntity(userId);
            await _offerRepo.AddAsync(offer);

            Product product = addRequest.ToProductEntity(offer);
            await _productRepo.AddAsync(product);

            await SaveNewImagesAsync(addRequest, product);
            await AddDeliveryTypesAsync(addRequest, offer);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result> Edit(OfferUpdateRequest? updateRequest)
        {
            if (updateRequest == null)
                return Result.Failure(OfferErrors.OfferIsNull);

            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetOfferWithDetailsToEditAsync(updateRequest.Id, userId);

            if (offer == null)
                return Result.Failure(OfferErrors.OfferDoesNotExist);

            offer.IsOfferPrivate = updateRequest.IsOfferPrivate;
            offer.StockQuantity = updateRequest.StockQuantity;
            offer.Price = updateRequest.Price;

            Product product = offer.Product;
            product.ProductName = updateRequest.ProductName;
            product.Description = updateRequest.Description;
            product.ConditionId = updateRequest.SelectedProductCondition;
            product.ProductCategoryId = updateRequest.SelectedProductCategory;

            //deletes images checked by user
            if (updateRequest.ImagesToDelete?.Count > 0)
            {
                var result = _productImageService.DeleteImagesFromOffer(product.ProductImages, updateRequest.ImagesToDelete);

                if (result.IsFailure)
                    return Result.Failure(result.Error);
            }

            await SaveNewImagesAsync(updateRequest, product);

            //clean existing deliveries for offer
            offer.OfferDeliveryTypes.Clear();

            //add new ones
            await AddDeliveryTypesAsync(updateRequest, offer);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteOffer(int id)
        {
            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetUserOfferByIdAsync(id, userId);

            if (offer == null)
                return Result.Failure(OfferErrors.OfferDoesNotExist);

            offer.DateDeleted = DateTime.UtcNow;
            offer.IsActive = false;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<IEnumerable<UserOfferResponse>> GetFilteredUserOffers(string? title)
        {
            Guid userId = _currentUserService.GetUserId();
            var offers = await _offerRepo.GetFilteredUserOffersAsync(title, userId);
            return offers.Select(item => item.ToUserOfferResponse());
        }

        public async Task<Result<EditOfferResponse>> GetOfferForEdit(int id)
        {
            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetOfferWithAllDetailsByUserAsync(id, userId);
            if (offer == null)
                return Result.Failure<EditOfferResponse>(OfferErrors.OfferDoesNotExist);

            return offer.ToEditOfferResponse();
        }

        public async Task<IEnumerable<OfferIndexResponse>> GetFilteredOffers(OfferFilter filter)
        {
            var offers = await _offerRepo.GetFilteredOffersAsync(filter);
            return offers.Select(item => item.ToOfferIndexResponse());
        }

        public async Task<IEnumerable<CardResponse>> GetIndexPageOffers()
        {
            var offers = await _offerRepo.GetOffersByTakeAsync();
            return offers.Select(item => item.ToCardResponse());
        }

        public async Task<bool> DoesOfferExist(int id)
        {
            return await _offerRepo.IsOfferInDbAsync(id);
        }

        public async Task<IEnumerable<CardResponse>> GetDealsOfTheDay()
        {
            var count = await _offerRepo.GetNonPrivateOfferCount();
            if (count == 0)
                return [];

            var take = Math.Min(count, 7);

            var offers = await _offerRepo.GetOffersByTakeAsync(take);

            return offers.Select(item => item.ToCardResponse());
        }

        public async Task<Result<OfferResponse>> GetOffer(int id)
        {
            var offer = await _offerRepo.GetOfferWithAllDetailsAsync(id);

            if (offer == null || offer.IsOfferPrivate)
                return Result.Failure<OfferResponse>(OfferErrors.OfferDoesNotExist);

            var userId = _currentUserService.GetCurrentUserIdOrNull();
            return offer.ToOfferResponse(userId);
        }

        private async Task SaveNewImagesAsync(IOfferImageDto dto, Product product)
        {
            dto.UploadedImagesUrls = await _pictureHandlerService.SavePicturesToDirectory(dto.UploadedImages);

            if (dto.UploadedImagesUrls?.Count > 0)
            {
                var newImages = dto.UploadedImagesUrls.Select(url => new ProductImage
                {
                    ImagePath = url,
                    IsActive = true,
                    DateCreated = DateTime.UtcNow
                });

                foreach (var image in newImages)
                {
                    product.ProductImages.Add(image);
                }
            }
        }

        private async Task AddDeliveryTypesAsync(IOfferDeliveryDto dto, Offer offer)
        {
            if (dto.SelectedParcelLocker.HasValue)
            {
                var parcelLockerDelivery = dto.ToOfferDeliveryTypeEntity(offer);
                await _offerDeliveryTypeRepo.AddAsync(parcelLockerDelivery);
            }

            var deliveryTypes = dto.SelectedOtherDeliveries
                .Select(deliveryId => deliveryId.ToOfferDeliveryTypeEntity(offer));

            await _offerDeliveryTypeRepo.AddRangeAsync(deliveryTypes);
        }
    }
}