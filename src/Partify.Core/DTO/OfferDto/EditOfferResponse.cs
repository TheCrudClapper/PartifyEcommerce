using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.DTO.OfferDto
{
    public class EditOfferResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string SelectedProductCategory { get; set; } = null!;
        public bool IsOfferPrivate { get; set; }
        public string SelectedProductCondition { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int? SelectedParcelLocker { get; set; }
        public List<int> SelectedOtherDeliveries { get; set; } = new List<int>();
        public List<SelectListItemDto>? ExistingImagesUrls { get; set; } = new List<SelectListItemDto>();

    }
}
