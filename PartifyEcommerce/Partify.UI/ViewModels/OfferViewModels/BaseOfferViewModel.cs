using System.ComponentModel.DataAnnotations;
using CSOS.UI.CustomValidators;
using CSOS.UI.ViewModels.DeliveryTypeViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.ViewModels.OfferViewModels
{
    /// <summary>
    /// This base class contains elements that are the same for edit and add view offer view models
    /// </summary>
    public abstract class BaseOfferViewModel
    {
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Product Category is required")]
        public string SelectedProductCategory { get; set; } = null!;

        public bool IsOfferPrivate { get; set; } = default!;

        [Required(ErrorMessage = "Product Condition is required")]
        public string SelectedProductCondition { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be at least 1 PLN")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int StockQuantity { get; set; }

        public List<IFormFile>? UploadedImages { get; set; }
        public int? SelectedParcelLocker { get; set; }

        //Holds multiple selected deliveries 
        [EmptyListValidator(ErrorMessage = "Those deliveries are required")]
        public List<int> SelectedOtherDeliveries { get; set; } = new List<int>();

        //Collections for holding select list data
        [BindNever]
        public IEnumerable<DeliveryTypeViewModel> ParcelLockerDeliveriesList { get; set; } = [];

        [BindNever]
        public IEnumerable<SelectListItem> ProductConditionsSelectList { get; set; } = [];

        [BindNever]
        public IEnumerable<SelectListItem> ProductCategoriesSelectionList { get; set; } = [];

        [BindNever]
        public IEnumerable<SelectListItem> OtherDeliveriesSelectedList { get; set; } = [];
    }
}
