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
            if(context.ActionArguments.TryGetValue("viewModel", out var arg) && arg is BaseOfferViewModel vm)
            {
                switch (vm)
                {
                    case AddOfferViewModel addVm:
                        if (addVm.UploadedImages == null)
                        {
                            context.ModelState.AddModelError(nameof(addVm.UploadedImages), "You have to add at least one photo");
                        }   
                        else
                        {
                            var extensionValidationResult = _pictureHandlerService.CheckFileExtensions(addVm.UploadedImages);
                            if(extensionValidationResult != "OK")
                                context.ModelState.AddModelError("WrongFileType", extensionValidationResult);
                        }
                        break;

                    case EditOfferViewModel editVm:
                        int remaining = editVm.ExistingImagesCount - (editVm.ImagesToDelete?.Count ?? 0);
                        bool hasNew = editVm.UploadedImages != null && editVm.UploadedImages.Any();

                        if (remaining < 1 && !hasNew)
                        {
                            context.ModelState.AddModelError(nameof(editVm.UploadedImages), "Offer must have at least one image left.");
                        }

                        if (editVm.UploadedImages != null)
                        {
                            var extensionValidationResult = _pictureHandlerService.CheckFileExtensions(editVm.UploadedImages);
                            if (extensionValidationResult != "OK")
                                context.ModelState.AddModelError("WrongFileType", extensionValidationResult);
                        }

                        break;
                }

            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
