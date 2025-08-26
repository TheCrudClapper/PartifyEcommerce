using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.ResultTypes;

namespace CSOS.Core.ServiceContracts
{

    public interface IProductImageService
    {
        /// <summary>
        /// Gets pictures for specific offer
        /// </summary>
        /// <param name="offerId">Id of offer to fetch pictures from</param>
        /// <returns>Returns List of SelectListItemDto</returns>
        Task<IEnumerable<SelectListItemDto>> GetOfferPicturesAsync(int offerId);
        
        /// <summary>
        /// Removes specific images from an offer.
        /// </summary>
        /// <param name="images">List of product images</param>
        /// <param name="imageUrls">List of image URLs to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Result DeleteImagesFromOffer(IEnumerable<ProductImage> images, IEnumerable<int>? imageIds);
    }
}
