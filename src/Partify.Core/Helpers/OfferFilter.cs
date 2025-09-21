namespace CSOS.Core.Helpers
{
    public class OfferFilter
    {
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string? DeliveryOption { get; set; }
        public string? SortOption { get; set; }
        public string? SearchPhrase { get; set; }
        public int? CategoryId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public override string ToString()
        {
            return $"Price From: {PriceFrom}\nPrice To: {PriceTo}\n" +
                $"DeliveryOption: {DeliveryOption}\nSortOption{SortOption}\nSearchPhrase: {SearchPhrase}\nCategoryId: {CategoryId}";
        }
    }
}
