using DistributedECommerce.Orders.API.Services;
using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Application.Configurations;
using DistributedECommerce.Orders.Infrastructure.Messaging;
using DistributedECommerce.Warehouse.ApiClient.Configurations;

namespace DistributedECommerce.Orders.API.Extensions
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
            services.AddSingleton(rabbitMqConfiguration);
            services.AddScoped<IMessageSender, RabbitMqMessageSender>();
            return services;

        }
    }
}
