﻿using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for accessing product image data.
    /// </summary>
    public interface IProductImageRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of all product images associated with a specific offer.
        /// </summary>
        /// <param name="offerId">The ID of the offer to retrieve images for.</param>
        /// <returns>A collection of <see cref="ProductImage"/> entities.</returns>
        Task<IEnumerable<ProductImage>> GetImagesFromOfferAsync(int offerId);
    }
}
