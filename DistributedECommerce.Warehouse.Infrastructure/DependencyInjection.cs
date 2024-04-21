using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Infrastructure.Data;
using BoxCommerce.Warehouse.Infrastructure.Data.Interceptors;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace BoxCommerce.Warehouse.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            services.AddScoped<AuditableEntityInterceptor>();
            services.AddScoped<AuditTrailInterceptor>();
            services.AddScoped<DispatchDomainEventsInterceptor>();

            services.AddDbContext<IWarehouseDbContext, WarehouseDbContext>((sp, options) =>
            {
                options.AddInterceptors
                (
                    sp.GetService<AuditableEntityInterceptor>(),
                    sp.GetService<DispatchDomainEventsInterceptor>(),
                    sp.GetService<AuditTrailInterceptor>()
                );
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

            return services;
        }
    }
}
