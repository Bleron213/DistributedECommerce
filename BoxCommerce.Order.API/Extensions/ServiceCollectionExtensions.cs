using BoxCommerce.Orders.API.Services;
using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Application.Configurations;
using BoxCommerce.Orders.Infrastructure.Messaging;
using BoxCommerce.Warehouse.ApiClient.Configurations;

namespace BoxCommerce.Orders.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }

        public static IServiceCollection AddMessageQueue(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqConfiguration = new RabbitMqConfiguration();
            configuration.Bind("RabbitMqConfiguration", rabbitMqConfiguration);
            services.AddScoped<IMessageSender>(provider =>
            {
                return new RabbitMqMessageSender(rabbitMqConfiguration);
            });
            return services;

        }
    }
}
