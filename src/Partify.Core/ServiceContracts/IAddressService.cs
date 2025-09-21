using CSOS.Core.DTO.AddressDto;
using CSOS.Core.ResultTypes;

namespace CSOS.Core.ServiceContracts
{
    public interface IAddressService
    {
        /// <summary>
        /// Adds a new address based on the provided request.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="request"/> contains valid address details before
        /// calling this method. The operation may fail if the request data is incomplete or invalid.</remarks>
        /// <param name="request">The request containing the details of the address to be added.  This parameter can be <see
        /// langword="null"/>; if <see langword="null"/>, the operation will fail.</param>
        /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation.  The result contains the outcome of
        /// the operation, including success or failure details.</returns>
        Task<Result> AddAddress(AddressAddRequest? request);

        /// <summary>
        /// Retrieves the address associated with the specified address ID.
        /// </summary>
        /// <param name="addressId">The unique identifier of the address to retrieve. If <see langword="null"/>, the method will return a result
        /// indicating that no address was found.</param>
        /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation. The result contains the address data
        /// if found, or an error message if the address ID is invalid or the address does not exist.</returns>
        Task<Result<AddressResponse>> GetAddress(int addressId);

        /// <summary>
        /// Updates the user's address with the specified ID.
        /// </summary>
        /// <param name="request">Updated address data.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> EditUserAddress(AddressUpdateRequest? request);

        /// <summary>
        /// Retrieves address details for editing by the currently logged-in user.
        /// </summary>
        /// <returns>
        /// </returns>
        Task<Result<AddressResponse>> GetUserAddressForEdit();

        /// <summary>
        /// Retrieves address information of the currently logged-in user.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{T}"/> containing <see cref="UserAddressDetailsResponse"/> if successful.
        /// </returns>
        Task<Result<UserAddressDetailsResponse>> GetUserAddressDetails();
    }
}
