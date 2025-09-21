using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSOS.Core.Domain.Entities
{
    public class LikedOffer : BaseModel
    {
        public Guid UserId { get; set; }
        public int OfferId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; } = null!;
    }
}
