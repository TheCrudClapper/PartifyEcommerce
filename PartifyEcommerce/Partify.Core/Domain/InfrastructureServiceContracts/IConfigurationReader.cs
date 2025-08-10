namespace CSOS.Core.Domain.InfrastructureServiceContracts
{
    /// <summary>
    /// Provides configuration settings for application resources, such as default image placeholders and directory
    /// paths for product and category pictures.
    /// </summary>
    /// <remarks>This interface defines properties for accessing and modifying configuration values related to
    /// image placeholders and directory paths. Implementations of this interface are expected to provide mechanisms for
    /// retrieving and updating these settings.</remarks>
    public interface IConfigurationReader
    {
        public string DefaultPicturePlaceholder { get; set; }

        public string DefaultProductsPicturesDirectory { get; set; }

        public string DefaultCategoryPicturesDirectory { get; set; }
    }
}

