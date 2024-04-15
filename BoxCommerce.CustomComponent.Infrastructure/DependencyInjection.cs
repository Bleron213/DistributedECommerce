using BoxCommerce.CustomComponent.Application.Common.Infrastructure;
using BoxCommerce.CustomComponent.Infrastructure.Data;
using BoxCommerce.CustomComponent.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoxCommerce.CustomComponent.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            services.AddScoped<AuditableEntityInterceptor>();
            services.AddScoped<AuditTrailInterceptor>();

            services.AddDbContext<IBoxCommerceOrderDbContext, BoxCommerceOrderDbContext>((sp, options) =>
            {
                options.AddInterceptors
                (
                    sp.GetService<AuditableEntityInterceptor>(),
                    sp.GetService<AuditTrailInterceptor>()
                );
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
