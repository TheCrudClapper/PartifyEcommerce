using System.ComponentModel.DataAnnotations.Schema;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;

namespace CSOS.Core.Domain.Entities
{
    /// <summary>
    /// This model represents an Cart, user can have only one cart
    /// </summary>
    public class Cart : BaseModel
    {
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalCartValue { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? MinimalDeliveryValue { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalItemsValue { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
