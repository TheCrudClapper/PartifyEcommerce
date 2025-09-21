using Microsoft.AspNetCore.Http;

namespace CSOS.Core.DTO.DtoContracts
{
    /// <summary>
    /// Represents a contract for DTOs that contain uploaded image data.
    /// Enables shared image handling logic in both add and edit operations.
    /// </summary>
    public interface IOfferImageDto
    {
        /// <summary>
        /// List of uploaded image files
        /// </summary>
        List<IFormFile> UploadedImages { get; set; }

        /// <summary>
        /// List of URL paths to saved images
        /// </summary>
        List<string> UploadedImagesUrls { get; set; }
    }
}
