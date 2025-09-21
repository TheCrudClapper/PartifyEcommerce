using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.Entities
{
    public class Country : BaseModel
    {
        public string CountryName { get; set; } = null!;
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
