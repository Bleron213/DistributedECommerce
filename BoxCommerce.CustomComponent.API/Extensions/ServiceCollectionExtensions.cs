using BoxCommerce.CustomComponent.API.Services;
using BoxCommerce.CustomComponent.Application.Common.Infrastructure;

namespace BoxCommerce.CustomComponent.API.Extensions
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
