namespace CSOS.Core.ResultTypes
{
    public static class ProductImageErrors
    {
        public static readonly Error ProductImageIsNull = new Error(
           "ProductImage.ProductImageIsNull", "Given offer doesnt have any images");

        public static readonly Error ProductImagesAreEmpty = new Error(
           "ProductImage.ProductImagesAreEmpty", "Product images are empty");

        public static Error GivenProductImageNotFound(string name) => new Error(
            "ProductImage.GivenProductImageNotFound", $"Image of {name} not found, cannot delete");

        public static Error WrongImageExtension(string[] allowedExtensions) => new Error(
            "ProductImage.WrongImageExtension", $"Images should be only in formats {string.Join(',', allowedExtensions)}");

        public static Error AddAtLeastOneImage(string[] allowedExtensions) => new Error(
            "ProductImage.AddAtLeastOneImage", $"Add at least one image with allowed extensions: {string.Join(',', allowedExtensions)}");
    }
}
