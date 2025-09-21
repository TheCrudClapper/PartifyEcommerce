using CSOS.UI.ViewModels.CartItemViewModels;

namespace CSOS.UI.ViewModels.CartViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal? TotalCartValue { get; set; }
        public decimal? TotalDeliveryValue { get; set; }
        public decimal? TotalItemsValue{ get; set; }

    }
}
