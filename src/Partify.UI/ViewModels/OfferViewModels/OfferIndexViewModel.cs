using CSOS.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.ViewModels.OfferViewModels
{
    public class OfferIndexViewModel
    {
        public List<OfferIndexItemViewModel> Items = new();
        public OfferFilter Filter { get; set; } = null!;

        public List<SelectListItem> DeliveryOptions = new();

        public List<SelectListItem> SortingOptions = new();
    }
}
