using CSOS.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.ViewComponents
{
    public class NavbarCartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        public NavbarCartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int itemCount = await _cartService.GetCartItemsQuantity();
            return View(itemCount);
        }
    }
}
