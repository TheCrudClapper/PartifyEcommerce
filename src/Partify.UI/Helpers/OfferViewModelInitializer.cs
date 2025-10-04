using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.OfferViewModels;

namespace CSOS.UI.Helpers;

public class OfferViewModelInitializer
{
    private readonly ICategoryGetterService _categoryGetterService;
    private readonly IConditionGetterService _conditionGetterService;
    private readonly IDeliveryTypeGetterService _deliveryTypeGetterService;
    private readonly IPictureHandlerService _pictureHandlerService;
    private readonly IProductImageService _productImageService;
    public OfferViewModelInitializer(ICategoryGetterService categoryGetterService,
        IConditionGetterService conditionGetterService,
        IDeliveryTypeGetterService deliveryTypeGetterService,
        IProductImageService productImageService,
        IPictureHandlerService pictureHandlerService)
    {
        _categoryGetterService = categoryGetterService;
        _conditionGetterService = conditionGetterService;
        _deliveryTypeGetterService = deliveryTypeGetterService;
        _productImageService = productImageService;
        _pictureHandlerService = pictureHandlerService;
    }
    public async Task InitializeAllAsync<TViewModel>(TViewModel viewModel, CancellationToken cancellationToken = default)
        where TViewModel: BaseOfferViewModel
    {
        await PopulateOtherDeliveryTypes(viewModel, cancellationToken);
        await PopulateParcelLockerDeliveries(viewModel, cancellationToken);
        await PopulateCategories(viewModel, cancellationToken);
        await PopulateConditions(viewModel, cancellationToken);

    }

    public async Task GetOfferPicturesAsync(EditOfferViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var images = await _productImageService
            .GetOfferPicturesAsync(viewModel.Id, cancellationToken);

        viewModel.ExistingImagesUrls = images
            .ToSelectListItemPicture(_pictureHandlerService);

        viewModel.ExistingImagesCount = viewModel.ExistingImagesUrls.Count;
    }   

    public async Task PopulateParcelLockerDeliveries<TViewModel>(TViewModel viewModel, CancellationToken cancellationToken = default)
        where TViewModel : BaseOfferViewModel
    {
        viewModel.ParcelLockerDeliveriesList =
            (await _deliveryTypeGetterService.GetParcelLockerDeliveryTypes(cancellationToken))
            .Select(item => item.ToDeliveryTypeViewModel());
    }

    public async Task PopulateConditions<TViewModel>(TViewModel viewModel, CancellationToken cancellationToken = default)
        where TViewModel : BaseOfferViewModel
    {
        viewModel.ProductConditionsSelectList =
            (await _conditionGetterService.GetProductConditionsAsSelectList(cancellationToken))
            .ToSelectListItem();
    }

    public async Task PopulateCategories<TViewModel>(TViewModel viewModel, CancellationToken cancellationToken = default) 
        where TViewModel : BaseOfferViewModel
    {
        viewModel.ProductCategoriesSelectionList =
            (await _categoryGetterService.GetProductCategoriesAsSelectList(cancellationToken))
            .ToSelectListItem();
    }

    public async Task PopulateOtherDeliveryTypes<TViewModel>(TViewModel viewModel, CancellationToken cancellationToken = default) 
        where TViewModel : BaseOfferViewModel
    {
        viewModel.OtherDeliveriesSelectedList =
            (await _deliveryTypeGetterService.GetOtherDeliveryTypes(cancellationToken))
            .ToSelectListItem();
    }

}
