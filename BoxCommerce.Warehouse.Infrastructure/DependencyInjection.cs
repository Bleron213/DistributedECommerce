using BoxCommerce.Warehouse.Application.Common.Application.Services;
using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Application.Services;
using BoxCommerce.Warehouse.Infrastructure.Data;
using BoxCommerce.Warehouse.Infrastructure.Data.Interceptors;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IComponentHashingService, ComponentHashingService>();

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
