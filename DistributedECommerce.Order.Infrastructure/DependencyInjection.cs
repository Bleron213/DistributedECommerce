using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Infrastructure.Data;
using DistributedECommerce.Orders.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Orders.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            services.AddScoped<AuditableEntityInterceptor>();
            services.AddScoped<AuditTrailInterceptor>();
            services.AddScoped<DispatchDomainEventsInterceptor>();

            services.AddDbContext<IOrderDbContext, OrderDbContext>((sp, options) =>
            {
                options.AddInterceptors
                (
                    sp.GetService<AuditableEntityInterceptor>(),
                    sp.GetService<AuditTrailInterceptor>(),
                    sp.GetService<DispatchDomainEventsInterceptor>()
                );
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
