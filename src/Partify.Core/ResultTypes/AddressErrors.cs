namespace CSOS.Core.ResultTypes
{
    public static class AddressErrors
    {
        public static readonly Error AddressNotFound = new Error(
            "Address.AddressNotFound", "Address of given id not found");

        public static readonly Error MissingAddressData = new Error (
            "Address.MissingAddressData", "User's address details could not be loaded.");
        
        public static readonly Error MissingAddressUpdateRequest = new Error (
            "Address.MissingAddressUpdateRequest", "User's address update request not found");

        public static readonly Error AddressAddRequestIsNull = new Error(
            "Address.AddressAddRequestIsNull", "Address add request is null");
    }
}
