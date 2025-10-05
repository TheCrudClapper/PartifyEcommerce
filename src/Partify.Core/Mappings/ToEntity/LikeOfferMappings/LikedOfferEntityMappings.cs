using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Mappings.ToEntity.LikeOfferMappings;

public static class LikedOfferEntityMappings
{
    public static LikedOffer ToLikeOffer(Guid userId, int offerId)
    {
        return new LikedOffer()
        {
            OfferId = offerId,
            UserId = userId
        };
    }
}
