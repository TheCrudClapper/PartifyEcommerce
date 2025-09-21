using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CSOS.Core.ResultTypes;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace CSOS.Core.Services
{
    public class CurrentUserService : ICurrentUserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager
            )
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _userManager = userManager;
        }

        public async Task<Result<ApplicationUser>> GetCurrentUserAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user != null ? Result.Success(user) : Result.Failure<ApplicationUser>(AccountErrors.AccountNotFound);
        }

        public Guid? GetCurrentUserIdOrNull()
        {
            var httpContext  = _httpContextAccessor.HttpContext;
            var userIdClaim = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return null;

            return userId;
        }

        public Guid GetUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null || httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("No HTTP context or unauthenticated user.");

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("NameIdentifier claim is missing.");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid user ID format.");

            return userId;
        }

    }
}
