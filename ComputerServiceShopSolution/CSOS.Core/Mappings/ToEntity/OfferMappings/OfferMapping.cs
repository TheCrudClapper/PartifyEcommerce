﻿using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;

namespace CSOS.Core.Mappings.ToEntity.OfferMappings
{
    public static class OfferMapping
    {
        public static Offer ToOfferEntity(this AddOfferDto dto, Product product, Guid userId)
        {
            return new Offer()
            {
                Product = product,
                IsActive = true,
                DateCreated = DateTime.Now,
                Price = dto.Price,
                SellerId = userId,
                StockQuantity = dto.StockQuantity,
                IsOfferPrivate = dto.IsOfferPrivate,
            };
        }
    }
}
