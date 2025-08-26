using CSOS.Core.DTO.DtoContracts;
using Microsoft.AspNetCore.Http;

namespace CSOS.Core.DTO.OfferDto
{
    public class OfferUpdateRequest : IOfferDeliveryDto, IOfferImageDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int SelectedProductCategory { get; set; }
        public bool IsOfferPrivate { get; set; }
        public int SelectedProductCondition { get; set; }
        public List<IFormFile> UploadedImages { get; set; } = null!;
        public List<string> UploadedImagesUrls { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<int> SelectedOtherDeliveries { get; set; } = null!;
        public List<int>? ImagesToDeleteIds { get; set; } = new List<int>();
        public int? SelectedParcelLocker { get; set; }
    }
}
