namespace CSOS.UI.ViewModels.HomePageViewModels
{
    public class ProductScrollerViewModel
    {
        public string IdPrefix { get; set; } = null!;
        public IEnumerable<MainPageCardViewModel> Items { get; set; } = new List<MainPageCardViewModel>();
    }
}
