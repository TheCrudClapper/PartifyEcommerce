using System.ComponentModel.DataAnnotations.Schema;

namespace CSOS.Core.Domain.Entities
{
    public class CartItem : BaseModel
    {
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = null!;

        public int OfferId { get; set; }
        [ForeignKey("OfferId")]
        public Offer Offer { get; set; } = null!;
        
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"CartId: {CartId}, OfferId: {OfferId}, Quantity: {Quantity}";
        }
    }

   
}
