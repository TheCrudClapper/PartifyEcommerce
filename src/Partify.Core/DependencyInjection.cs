using CSOS.Core.Helpers;
using CSOS.Core.ServiceContracts;
using CSOS.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Partify.Core.Helpers;

namespace CSOS.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreLayer(this IServiceCollection services)
        {
            // -----------------------------
            // Services
            // -----------------------------
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICountriesGetterService, CountryGetterService>();
            services.AddScoped<ICategoryGetterService, CategoryGetterService>();
            services.AddScoped<IDeliveryTypeGetterService, DeliveryTypeGetterService>();
            services.AddScoped<IConditionGetterService, ConditionGetterService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<ISortingOptionService, SortingOptionsService>();

            // -----------------------------
            // Custom Caching Helper
            // -----------------------------
            services.AddScoped<ICachingHelper, CachingHelper>();

            return services;
        }
    }
}
