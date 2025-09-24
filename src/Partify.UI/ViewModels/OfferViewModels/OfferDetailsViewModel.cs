using CSOS.UI.ViewModels.DeliveryTypeViewModels;
namespace CSOS.UI.ViewModels.OfferViewModels
{
    public class OfferDetailsViewModel
    {
        public int Id { get; set; }
        public string ProductCondition { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string Title { get; set; } = null!;
        public string Seller { get; set; } = null!;
        public string ProductCategory { get; set; } = null!;
        public int StockQuantity { get; set; }
        public bool IsSellerCompany { get; set; }
        public string Description { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Place { get; set; } = null!;
        public decimal Price { get; set; }
        public bool? IsLiked { get; set; }

        public List<string> ProductImages = [];

        public IEnumerable<DeliveryTypeViewModel> AvaliableDeliveryTypes = [];
    }
}
