using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.AccountDto;

namespace CSOS.Core.Mappings.ToDomainEntity.ApplicationUserMappings
{
    public static class ApplicationUserMappings
    {
        public static ApplicationUser ToApplicationUserEntity (this RegisterRequest request, Cart cart)
        {
            return new ApplicationUser()
            {
                FirstName = request.FirstName,
                Surname = request.Surname,
                UserName = request.Email,
                Cart = cart,
                DateCreated = DateTime.Now,
                IsActive = true,
                Email = request.Email,
            };
           
        }

        public static string ToRoleName(this UserRoleOption userRole)
            => userRole.ToString();
    }
}
