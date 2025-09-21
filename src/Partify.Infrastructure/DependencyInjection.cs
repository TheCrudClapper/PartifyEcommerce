using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using ComputerServiceOnlineShop.Services;
using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Infrastructure.DbContext;
using CSOS.Infrastructure.Repositories;
using CSOS.UI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSOS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // -----------------------------
            // Db Context Config
            // -----------------------------
            services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ComputerServiceOnlineShop"),
                migrations => migrations.MigrationsAssembly("Partify.Infrastructure")));

            // -----------------------------
            // Identity Config
            // -----------------------------
            services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();

            // -----------------------------
            // Repositories and Services 
            // -----------------------------
            services.AddScoped<ILikeOfferRepository, LikeOfferRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IOfferDeliveryTypeRepository, OfferDeliveryTypeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IConditionRepository, ConditionRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IDeliveryTypeRepository, DeliveryTypeRepository>();
            services.AddScoped<IPictureHandlerService, PictureHandlerService>();
            services.AddScoped<IConfigurationReader, ConfigurationReader>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            return services;
        }
    }
}
