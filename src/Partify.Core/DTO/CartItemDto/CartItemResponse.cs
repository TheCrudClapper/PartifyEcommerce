namespace CSOS.Core.DTO.CartItemDto
{
    public class CartItemResponse
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public string Condition { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
