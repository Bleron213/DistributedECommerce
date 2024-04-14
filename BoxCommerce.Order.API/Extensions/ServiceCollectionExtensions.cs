using BoxCommerce.Orders.API.Services;
using BoxCommerce.Orders.Application.Common.Infrastructure;

namespace BoxCommerce.Orders.API.Extensions
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
