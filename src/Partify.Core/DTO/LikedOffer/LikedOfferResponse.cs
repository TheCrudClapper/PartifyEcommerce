namespace CSOS.Core.DTO.LikedOfferDto
{
    public record LikedOfferResponse(int Id, string Title, decimal Price, DateTime DateCreated, int StockQuantity, string? ProductImage);
}
