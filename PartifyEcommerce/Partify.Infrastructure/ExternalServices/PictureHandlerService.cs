using CSOS.Core.Domain.InfrastructureServiceContracts;
using Microsoft.AspNetCore.Http;
namespace ComputerServiceOnlineShop.Services
{
    public class PictureHandlerService : IPictureHandlerService
    {
        private string offerPicturesDirectory;
        private string categoriesPicturesDirectory;
        private string response = "";
        private string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

        private readonly IConfigurationReader _configurationReader;
        public PictureHandlerService(IConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
            offerPicturesDirectory = _configurationReader.DefaultProductsPicturesDirectory;
            categoriesPicturesDirectory = _configurationReader.DefaultCategoryPicturesDirectory;
        }

        public string CheckFileExtensions(List<IFormFile> uploadedImages)
        {
            if (uploadedImages != null && uploadedImages.Count > 0)
            {
                foreach (var image in uploadedImages)
                {
                    var extension = Path.GetExtension(image.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        response = $"Images should be only in formats {string.Join(',', allowedExtensions)}";
                        return response;
                    }
                }
            }
            else
            {
                response = $"Add at least one image in extendsion {string.Join(',', allowedExtensions)}";
                return response;
            }
            return response = "OK";
        }

        public async Task<List<string>> SavePicturesToDirectory(List<IFormFile> uploadedImages)
        {
            List<string> imagePaths = new List<string>();

            CreateDirIfNotExist(offerPicturesDirectory);

            if (uploadedImages != null && uploadedImages.Count > 0)
            {
                foreach (var file in uploadedImages)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName)
                            + "_" + Guid.NewGuid()
                            + Path.GetExtension(file.FileName).ToLower();

                        var filePath = Path.Combine(offerPicturesDirectory, fileName);


                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        imagePaths.Add($"/offer-images/{fileName}");
                    }
                }
            }
            return imagePaths;
        }

        public string ReplaceImageIfNotFound(string? filePath)
        {

            if (string.IsNullOrEmpty(filePath))
                return _configurationReader.DefaultPicturePlaceholder;

            //logic for product images
            if (filePath.StartsWith("/offer-images/", StringComparison.OrdinalIgnoreCase))
            {
                CreateDirIfNotExist(offerPicturesDirectory);
                var physicalPath = Path.Combine(offerPicturesDirectory, Path.GetFileName(filePath));
                if (!File.Exists(physicalPath))
                    return _configurationReader.DefaultPicturePlaceholder;
            }
            else if (filePath.StartsWith("/category-images/", StringComparison.OrdinalIgnoreCase))
            {
                CreateDirIfNotExist(categoriesPicturesDirectory);
                var physicalPath = Path.Combine(categoriesPicturesDirectory, Path.GetFileName(filePath));
                if (!File.Exists(physicalPath))
                    return _configurationReader.DefaultPicturePlaceholder;
            }
            else
            {
                return _configurationReader.DefaultPicturePlaceholder;
            }

            return filePath;
        }

        private void CreateDirIfNotExist(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

    }
}
