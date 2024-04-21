using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Application.Configurations;
using DistributedECommerce.Warehouse.Common.Enums;
using DistributedECommerce.Warehouse.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data;
using System.Text;

namespace DistributedECommerce.Warehouse.Application.BackgroundServices
{
    public class ComponentStateChangedConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        private IWarehouseDbContext _dbContext;
        private IDbConnection _db;


        public ComponentStateChangedConsumer(
            IServiceProvider services,
            IConfiguration configuration,
            RabbitMqConfiguration rabbitMqConfiguration
            )
        {
            _services = services;
            _configuration = configuration;
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqConfiguration.HostName,
                Password = rabbitMqConfiguration.Password,
                UserName = rabbitMqConfiguration.Username
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("component-completed", false, false, false, null);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var orderCreatedMessage = JsonConvert.DeserializeObject<ComponentStatusChange>(content);
                await HandleComponentStateChanged(orderCreatedMessage);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("component-completed", false, consumer);
        }

        private async Task HandleComponentStateChanged(ComponentStatusChange componentStatusChange)
        {
            using var scope = _services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<IWarehouseDbContext>();

            var component = await _dbContext.Components
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == componentStatusChange.ComponentId) ?? throw new Exception($"Could not find component with Id = {componentStatusChange.ComponentId}");

            component.ComponentStateChange((Domain.Enums.ComponentStatus)componentStatusChange.NewStatus);

            if(component.ProductId is not null)
            {
                var product = await _dbContext.Products.Include(x => x.Components).Where(x => x.Id == component.ProductId!).FirstAsync();
                var productOldStatus = product.Status;
                product.ProductStatusUpdateCheck();
                
                if(productOldStatus != product.Status)
                {
                    product.AddDomainEvent(new ProductStateChangedEvent(product));
                }
            }

            await _dbContext.SaveChangesAsync();

        }

        public class ComponentStatusChange
        {
            public Guid ComponentId { get; set; }
            public ComponentStatus NewStatus { get; set; }
        }
    }
}
