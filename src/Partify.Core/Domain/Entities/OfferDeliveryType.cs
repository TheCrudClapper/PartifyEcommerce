using System.ComponentModel.DataAnnotations.Schema;
using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.Entities
{
    public class OfferDeliveryType : BaseModel
    {
        public int OfferId { get; set; }
        public int DeliveryTypeId { get; set; }

        [ForeignKey("DeliveryTypeId")]
        public DeliveryType DeliveryType { get; set; } = null!;

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; } = null!;
    }
}
