namespace CSOS.Core.ResultTypes
{
    public static class LikeOfferErrors
    {
        public static readonly Error OwnOfferLiked = new Error("OfferLike.OwnOfferLiked",
            "You can't favourite your own offer !");
    }
}
