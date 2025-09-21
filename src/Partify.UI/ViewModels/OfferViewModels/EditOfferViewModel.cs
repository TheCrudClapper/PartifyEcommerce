using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.ViewModels.OfferViewModels
{
    /// <summary>
    /// View model for editing existing offer in shop
    /// </summary>
    public class EditOfferViewModel : BaseOfferViewModel
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }

        //Count of images downloaded from db
        public int ExistingImagesCount { get; set; }

        //Images taken from database
        [BindNever]
        public List<SelectListItem>? ExistingImagesUrls { get; set; } = new List<SelectListItem>();

        //Images selected by user for deletion
        public List<string>? ImagesToDeleteIds { get; set; } = new List<string>();
    }
}
