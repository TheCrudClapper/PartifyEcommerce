namespace CSOS.Core.Domain.Entities
{
    public class Condition : BaseModel
    {
        public string ConditionTitle { get; set; } = null!;
        public string ConditionDescription { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
