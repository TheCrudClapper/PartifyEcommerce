using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.ViewModels.HomePageViewModels
{
    public class IndexPageViewModel
    {
        public IEnumerable<MainPageCardViewModel> Cards = new List<MainPageCardViewModel>();
        public IEnumerable<SelectListItem> Categories = new List<SelectListItem>();
        public IEnumerable<MainPageCardViewModel> CategoriesSlider = new List<MainPageCardViewModel>();
        public IEnumerable<MainPageCardViewModel> BestDeals = new List<MainPageCardViewModel>();
        public string? SelectedCategory { get; set; }
    }
}
