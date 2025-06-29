﻿namespace ComputerServiceOnlineShop.Entities.Models
{
    public class Condition : BaseModel
    {
        public string ConditionTitle { get; set; } = null!;
        public string ConditionDescription { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
