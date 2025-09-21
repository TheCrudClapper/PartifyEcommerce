using CSOS.Core.DTO.CartItemDto;

namespace CSOS.Core.DTO.CartDto
{
    public class CartResponseDto
    {
        public IEnumerable<CartItemResponse> CartItems { get; set; } = new List<CartItemResponse>();
        public decimal? TotalCartValue { get; set; }
        public decimal? TotalDeliveryValue { get; set; }
        public decimal? TotalItemsValue { get; set; }
    }
}
