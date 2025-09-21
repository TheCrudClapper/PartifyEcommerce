using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.Universal;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.ViewComponents
{
    public class CategoryDropdownViewComponent : ViewComponent
    {
        private readonly ICategoryGetterService _categoryGetterService;
        public CategoryDropdownViewComponent(ICategoryGetterService categoryGetterService)
        {
            _categoryGetterService = categoryGetterService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = (await _categoryGetterService.GetProductCategoriesAsSelectList()).ToSelectListItem();
            return View(categories);
        }
    }
}
