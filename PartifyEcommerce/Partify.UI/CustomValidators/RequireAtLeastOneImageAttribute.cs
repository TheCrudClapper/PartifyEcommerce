using CSOS.UI.ViewModels.OfferViewModels;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Validation Attribure for EditOfferViewModel validating if at least one photo is left on offer,
/// if user attemps deletion of all photos
/// </summary>
public class RequireAtLeastOneImageAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var viewModel = validationContext.ObjectInstance as EditOfferViewModel;
        if (viewModel == null)
            return ValidationResult.Success;

        int remainingImages = viewModel.ExistingImagesCount - viewModel.ImagesToDelete?.Count() ?? 0;
        bool hasNewImages = viewModel.UploadedImages != null && viewModel.UploadedImages.Any();

        if (remainingImages < 1 && !hasNewImages)
        {
            return new ValidationResult("Offer must have at least one image left.", new[] { nameof(viewModel.UploadedImages) });
        }

        return ValidationResult.Success;
    }
}
