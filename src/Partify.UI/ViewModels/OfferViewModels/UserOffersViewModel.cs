namespace CSOS.UI.ViewModels.OfferViewModels
{
    public class UserOffersViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductCategory { get; set; } = null!;
        public string ProductCondition { get; set; } = null!;
        public bool IsOfferPrivate { get; set; }

        public int StockQuantity;
        
        public decimal Price;
        public DateTime DateCreated { get; set; }
    }
}
