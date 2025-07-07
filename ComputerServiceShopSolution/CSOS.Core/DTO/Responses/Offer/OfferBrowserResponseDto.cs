﻿using CSOS.Core.Helpers;

namespace CSOS.Core.DTO.Responses.Offers
{
    public class OfferBrowserResponseDto
    {
        public List<OfferBrowserItemResponseDto> Items = new List<OfferBrowserItemResponseDto>();
        public OfferFilter Filter { get; set; } = null!;

        public List<SelectListItemDto> DeliveryOptions = new List<SelectListItemDto>();

        public List<SelectListItemDto> SortingOptions = new List<SelectListItemDto>();
    }
}
