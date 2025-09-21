namespace CSOS.UI.ViewModels.CartItemViewModels
{ 
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public int Quantity { get; set; }
        public string Condition { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
