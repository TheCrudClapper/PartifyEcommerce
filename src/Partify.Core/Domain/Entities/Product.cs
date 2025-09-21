using System.ComponentModel.DataAnnotations.Schema;

namespace CSOS.Core.Domain.Entities
{
    public class Product : BaseModel
    {
        public string ProductName { get; set; } = null!;
        public int OfferId { get; set; }
        public string Description { get; set; } = null!;
        public int ProductCategoryId { get; set; }
        public int ConditionId { get; set; }

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; } = null!;

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategory { get; set; } = null!;

        [ForeignKey("ConditionId")]
        public Condition Condition { get; set; } = null!;
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
