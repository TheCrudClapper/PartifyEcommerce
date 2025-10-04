using CSOS.Core.ResultTypes;
using Microsoft.AspNetCore.Http;

namespace CSOS.Core.Domain.InfrastructureServiceContracts
{
    public interface IPictureHandlerService
    {
        /// <summary>
        /// Saves the provided list of uploaded images to a directory and returns the file paths of the saved images.
        /// </summary>
        /// <remarks>This method processes the provided images asynchronously and saves them to a
        /// predefined directory.  Ensure that the application has the necessary permissions to write to the target
        /// directory.</remarks>
        /// <param name="uploadedImages">A list of uploaded images to be saved. Each image must be a valid <see cref="IFormFile"/> object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of file paths where the
        /// images were saved.</returns>
        Task<List<string>> SavePicturesToDirectory(List<IFormFile> uploadedImages, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates the file extensions of the uploaded images.
        /// </summary>
        /// <remarks>This method checks the file extensions of the provided images against a predefined
        /// set of valid extensions. Ensure that the list contains only image files to avoid unexpected
        /// results.</remarks>
        /// <param name="uploadedImages">A list of uploaded files to validate. Each file must implement <see cref="IFormFile"/>.</param>
        /// <returns>A Result indicating the result of the validation. Returns an error message if any file has an invalid
        /// extension; otherwise, returns an empty string.</returns>
        Result CheckFileExtensions(List<IFormFile> uploadedImages);

        /// <summary>
        /// Replaces the specified image file path with a default placeholder if the file is not found.
        /// </summary>
        /// <remarks>This method checks whether the file at the specified path exists. If the file is not
        /// found, it returns a predefined placeholder image path. This can be useful for scenarios where a missing
        /// image needs to be replaced with a default visual representation.</remarks>
        /// <param name="filePath">The file path of the image to check. Can be <see langword="null"/> or empty.</param>
        /// <returns>The original file path if the file exists; otherwise, a default placeholder image path.</returns>
        string ReplaceImageIfNotFound(string? filePath);
    }
}
