namespace CSOS.Core.ResultTypes
{
    public static class CartItemErrors
    {
        public static readonly Error CartItemDoesNotExists = new Error(
            "CartItemDto.ItemDoesNotExists", "Item of given id does not exists");

        public static readonly Error QuantityLowerThanZero = new Error(
            "CartItemDto.QuantityLowerThanZero", "Quantity must be greater that zero !");

        public static readonly Error CannotAddMoreToCart = new Error(
            "CartItemDto.CannotAddMoreToCart", "Cannot add more than is in shop");

        public static readonly Error InvalidItemQuantity = new Error(
            "CartItemDto.InvalidItemQuantity", "Invalid item quantity");
    }
}
