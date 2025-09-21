namespace CSOS.Core.DTO.DtoContracts
{
    /// <summary>
    /// Represents a contract for DTOs that contain delivery method information.
    /// Enables shared processing of parcel locker and other delivery types.
    /// </summary>
    public interface IOfferDeliveryDto
    {
        /// <summary>
        /// Id of selected parcel locker
        /// </summary>
        int? SelectedParcelLocker { get; }
        /// <summary>
        /// List of ID's of other selcted delivery methods
        /// </summary>
        List<int> SelectedOtherDeliveries { get; }
    }
}
