﻿using CSOS.Core.DTO.DeliveryTypeDto;

namespace CSOS.Core.DTO.OfferDto
{
    public class OfferResponse
    {
        public int Id { get; set; }
        public string Condition { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string Category { get; set; } = null!;
        public bool IsSellerCompany { get; set; }
        public string Title { get; set; } = null!;
        public string Seller { get; set; } = null!;
        public int StockQuantity { get; set; }
        public string Description { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Place { get; set; } = null!;
        public decimal Price { get; set; }

        public List<string> ProductImages = [];

        public List<DeliveryTypeResponse> AvaliableDeliveryTypes = [];
    }
}
