using CSOS.Core.Domain.InfrastructureServiceContracts;
using Microsoft.Extensions.Configuration;

namespace CSOS.UI.Helpers
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly IConfiguration _configuration;

        public string DefaultPicturePlaceholder { get; set; } = null!;
        public string DefaultProductsPicturesDirectory { get; set; } = null!;
        public string DefaultCategoryPicturesDirectory { get; set; } = null!;

        public ConfigurationReader(IConfiguration configuration)
        {
            _configuration = configuration;
            DefaultPicturePlaceholder = configuration["ImageDirectories:DefaultPicturePlaceholder"] ?? "/images/no-image.png";
            DefaultProductsPicturesDirectory = configuration["ImageDirectories:DefaultProductPicturesDirectory"] ?? "/wwwroot/offer-images/";
            DefaultCategoryPicturesDirectory = configuration["ImageDirectories:DefaultCategoryPicturesDirectory"] ?? "/wwwroot/category-images/";
        }
    }
}
