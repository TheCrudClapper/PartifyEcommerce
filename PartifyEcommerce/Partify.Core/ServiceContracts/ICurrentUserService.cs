using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.ResultTypes;

namespace CSOS.Core.ServiceContracts
{
    public interface ICurrentUserService
    {
        /// <summary>
        /// Method serches for currently logged user
        /// </summary>
        /// <returns>Returns GUID of currently logged in user</returns>
        Guid GetUserId();

        /// <summary>
        /// Method searches for logged user in claims
        /// </summary>
        /// <returns>Return used Id if user found, otherwise null</returns>
        Guid? GetCurrentUserIdOrNull();

        /// <summary>
        /// Return currenty logged in application user object
        /// </summary>
        /// <returns>Returns currently logged in user</returns>
        Task<Result<ApplicationUser>> GetCurrentUserAsync();


    }
}
