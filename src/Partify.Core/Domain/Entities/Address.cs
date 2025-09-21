using System.ComponentModel.DataAnnotations.Schema;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;

namespace CSOS.Core.Domain.Entities
{
    public class Address : BaseModel
    {
        public string Place { get; set; } = null!;
        public Guid? UserId { get; set; }
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
