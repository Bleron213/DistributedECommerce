using BoxCommerce.Warehouse.API.Services;
using BoxCommerce.Warehouse.Application.Common.Infrastructure;

namespace BoxCommerce.Warehouse.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
