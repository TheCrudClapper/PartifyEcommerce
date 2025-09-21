namespace CSOS.Core.ResultTypes
{
    public static class OfferErrors
    {
        public static readonly Error OfferIsNull = new Error(
             "Offer.OfferIsNull", "Given offer is null");

        public static readonly Error OfferDoesNotExist = new Error(
            "Offer.OfferDoesNotExist", "Offer doesn not exists");
    }
}
