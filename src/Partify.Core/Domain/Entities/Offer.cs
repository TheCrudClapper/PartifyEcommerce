using System.ComponentModel.DataAnnotations.Schema;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;

namespace CSOS.Core.Domain.Entities
{
    /// <summary>
    /// Domain Model representing Offer in Online Shop
    /// </summary>
    public class Offer : BaseModel
    {

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsOfferPrivate { get; set; }
        public Guid SellerId { get; set; }

        [ForeignKey("SellerId")]
        public ApplicationUser Seller { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public ICollection<OfferDeliveryType> OfferDeliveryTypes { get; set; } = new List<OfferDeliveryType>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public ICollection<LikedOffer> LikedOffers { get; set; } = new List<LikedOffer>();
    }
}
