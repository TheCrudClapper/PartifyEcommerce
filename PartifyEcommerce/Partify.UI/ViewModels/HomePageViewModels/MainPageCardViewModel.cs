﻿namespace CSOS.UI.ViewModels.HomePageViewModels
{
    public class MainPageCardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
