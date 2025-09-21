using System.ComponentModel.DataAnnotations.Schema;
using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.Entities
{
    public class ProductImage : BaseModel
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
    }
}
