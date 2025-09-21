namespace CSOS.Core.DTO.OfferDto
{
    public class UserOfferResponse
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductCategory { get; set; } = null!;
        public string ProductCondition { get; set; } = null!;
        public bool IsOfferPrivate { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
