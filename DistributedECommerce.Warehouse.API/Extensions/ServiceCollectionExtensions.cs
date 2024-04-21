using DistributedECommerce.Warehouse.API.Services;
using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Application.Configurations;
using DistributedECommerce.Warehouse.Infrastructure.Messaging;

namespace DistributedECommerce.Warehouse.API.Extensions
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
