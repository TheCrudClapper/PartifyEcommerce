using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.UI.ViewModels.OfferViewModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CSOS.UI.Filters
{
    /// <summary>
    /// Filter that validates images sent for POST Create and Edit method in OfferController
    /// </summary>
    public class ValidatePicturesFilter : IActionFilter
    {
        private readonly IPictureHandlerService _pictureHandlerService;
        public ValidatePicturesFilter(IPictureHandlerService pictureHandlerService)
        {
            _pictureHandlerService = pictureHandlerService;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("viewModel", out var arg) && arg is BaseOfferViewModel vm)
            {
                switch (vm)
                {
                    case AddOfferViewModel addVm:
                        if (addVm.UploadedImages == null)
                            context.ModelState.AddModelError(nameof(addVm.UploadedImages), "You have to add at least one photo");
                        else
                            TryValidateUploadedImages(addVm, context);
                        break;

                    case EditOfferViewModel editVm:
                        bool validationResult = TryValidateUploadedImages(editVm, context); ;
                        bool hasAtLeastOneImage = HasAtLeastOneImage(editVm, validationResult);

                        if (!hasAtLeastOneImage)
                            context.ModelState.AddModelError(nameof(editVm.UploadedImages), "Offer must have at least one image left.");
                        break;
                }

            }
        }
        private bool HasAtLeastOneImage(EditOfferViewModel vm, bool newImagesValid)
        {
            int remaining = vm.ExistingImagesCount - (vm.ImagesToDelete?.Count ?? 0);
            bool hasNew = newImagesValid && vm.UploadedImages?.Any() == true;
            return remaining > 0 || hasNew;
        }
        private bool TryValidateUploadedImages(BaseOfferViewModel viewModel, ActionExecutingContext context)
        {
            if (viewModel.UploadedImages != null)
            {
                var validationResult = _pictureHandlerService.CheckFileExtensions(viewModel.UploadedImages);
                if (validationResult.IsFailure)
                {
                    context.ModelState.AddModelError("WrongFileType", validationResult.Error.Description);
                    return false;
                }
                return true;
            }
            return false;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
