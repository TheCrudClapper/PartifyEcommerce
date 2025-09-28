namespace CSOS.Core.DTO.OfferDto
{
    public class OfferIndexResponse
    {
        public int Id { get; set; }
        public string ProductCondition { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string ProductCategory { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string Description { get; set; } = null!;
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string Seller { get; set; } = null!;
    }
}