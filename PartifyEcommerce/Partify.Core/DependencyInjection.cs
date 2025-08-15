using CSOS.Core.ServiceContracts;
using CSOS.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CSOS.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreLayer(this IServiceCollection services)
        {
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
            return services;
        }
    }
}
