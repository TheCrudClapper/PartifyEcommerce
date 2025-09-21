namespace CSOS.Core.DTO.UniversalDto
{
    public class CardResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
