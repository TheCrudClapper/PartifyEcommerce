using CSOS.Core.DTO.AccountDto;
using CSOS.Core.ResultTypes;
using Microsoft.AspNetCore.Identity;

namespace CSOS.Core.ServiceContracts
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers a new user based on the provided registration details.
        /// </summary>
        /// <remarks>This method performs user registration and returns an <see cref="IdentityResult"/>
        /// that provides  details about the success or failure of the operation. Ensure that all required fields in the
        /// <paramref name="request"/> object are populated before calling this method.</remarks>
        /// <param name="request">The registration details for the new user. This includes information such as username, password,  and other
        /// required fields. Cannot be null.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result contains an  <see
        /// cref="IdentityResult"/> indicating whether the registration was successful.</returns>
        Task<IdentityResult> Register(RegisterRequest? request);

        /// <summary>
        /// Attempts to sign in a user using the provided login request.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="request"/> contains valid credentials and any
        /// required fields before calling this method. The behavior of the method may vary depending on the
        /// implementation of the login process, such as handling multi-factor authentication or account lockout
        /// scenarios.</remarks>
        /// <param name="request">The login request containing user credentials and other sign-in details. This parameter can be null.</param>
        /// <returns>A <see cref="SignInResult"/> object representing the outcome of the login attempt.  The result indicates
        /// whether the sign-in was successful and may include additional information about the sign-in process.</returns>
        Task<SignInResult> Login(LoginRequest? request);

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        Task Logout();

        /// <summary>
        /// Retrieves account details for editing purposes.
        /// </summary>
        /// <remarks>The returned account details are intended to be used for editing operations. Ensure
        /// that the caller has appropriate permissions to edit the account.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/> object
        /// wrapping an <see cref="AccountResponse"/> with the account details.</returns>
        Task<Result<AccountResponse>> GetAccount();

        /// <summary>
        /// Updates the currently logged-in user's account details.
        /// </summary>
        /// <param name="request">Updated account information.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure of the update.</returns>
        Task<Result> Edit(AccountUpdateRequest? request);

        /// <summary>
        /// Changes the password of the currently logged-in user.
        /// </summary>
        /// <param name="request">Password change data including current and new passwords.</param>
        /// <returns>A <see cref="Result"/> indicating the outcome of the operation.</returns>
        Task<Result> ChangePassword(PasswordChangeRequest? request);

        /// <summary>
        /// Retrieves detailed account information of the currently logged-in user.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing <see cref="AccountDetailsResponse"/> if successful.</returns>
        Task<Result<AccountDetailsResponse>> GetAccountDetailsAsync();

        /// <summary>
        /// Determines whether the current user has an associated address.
        /// </summary>
        /// <remarks>This method performs an asynchronous check to determine if the current user has an
        /// address  associated with their profile. The result can be used to conditionally display or enable 
        /// address-related functionality.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/>  if the
        /// current user has an address; otherwise, <see langword="false"/>.</returns>
        Task<bool> DoesCurrentUserHaveAddress();

        /// <summary>
        /// Determines whether the specified email address is already in use.
        /// </summary>
        /// <param name="email">The email address to check. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains  <see langword="true"/> if the
        /// email address is already taken; otherwise, <see langword="false"/>.</returns>
        Task<bool> IsEmailAlreadyTaken(string email);
    }
}
