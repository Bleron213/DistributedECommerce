using DistributedECommerce.Warehouse.API.Services;
using DistributedECommerce.Warehouse.Application.Common.Infrastructure;

namespace DistributedECommerce.Warehouse.API.Extensions
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
