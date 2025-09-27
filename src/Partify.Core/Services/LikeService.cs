using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.LikedOfferDto;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services;

public class LikeService : ILikeService
{
    private readonly IOfferRepository _offerRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILikeOfferRepository _likeOfferRepository;
    public LikeService(ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        ILikeOfferRepository likeOfferRepository,
        IOfferRepository offerRepository)
    {
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _likeOfferRepository = likeOfferRepository;
        _offerRepository = offerRepository;
    }

    public async Task<Result<IEnumerable<LikedOfferResponse>>> GetFilteredUserLikedOffers(string? title)
    {
        var userId = _currentUserService.GetCurrentUserIdOrNull();
        if (userId == null)
            return Result.Failure<IEnumerable<LikedOfferResponse>>(AccountErrors.UserIsNotLoggedIn);

        var likedOffers = await _likeOfferRepository.GetAllUserLikedOffersAsync(userId.Value, title);

        return Result.Success(likedOffers.Select(item => item.ToLikeOfferResponse()));
    }

    public async Task<Result<LikeResult>> ToggleLike(int offerId)
    {
        var userId = _currentUserService.GetUserId();

        var offer = await _offerRepository.GetOfferByIdAsync(offerId);

        if (offer == null)
            return Result.Failure<LikeResult>(OfferErrors.OfferDoesNotExist);

        if (offer.SellerId == userId)
            return Result.Failure<LikeResult>(LikeOfferErrors.OwnOfferLiked);

        var existing = await _likeOfferRepository.GetLikedOfferAsync(offerId, userId);

        if (existing != null && existing.IsActive)
        {
            await _likeOfferRepository.RemoveLikeAsync(existing);
            await _unitOfWork.SaveChangesAsync();
            return new LikeResult(false, "Unliked this offer");
        }

        var likedOffer = new LikedOffer
        {
            OfferId = offerId,
            UserId = userId,
        };

        await _likeOfferRepository.AddOrReactivateLikeAsync(likedOffer);
        await _unitOfWork.SaveChangesAsync();
        return new LikeResult(true, "Liked this offer!");
    }
}
