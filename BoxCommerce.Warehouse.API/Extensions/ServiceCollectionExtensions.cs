using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.API.Services;

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
